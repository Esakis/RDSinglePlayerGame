using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedDragonAPI.Data;
using RedDragonAPI.Models;
using RedDragonAPI.Helpers;

namespace RedDragonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BattleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private static readonly Random _random = new();

        public BattleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("monsters")]
        public async Task<ActionResult<List<MonsterDto>>> GetMonsters()
        {
            var monsters = await _context.Monsters
                .OrderBy(m => m.Level)
                .Select(m => new MonsterDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Level = m.Level,
                    MaxHp = m.MaxHp,
                    ExperienceReward = m.ExperienceReward,
                    GoldRewardMin = m.GoldRewardMin,
                    GoldRewardMax = m.GoldRewardMax
                })
                .ToListAsync();

            return Ok(monsters);
        }

        [HttpPost("{characterId}/fight/{monsterId}")]
        public async Task<ActionResult<BattleResultDto>> Fight(int characterId, int monsterId)
        {
            var character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Armor)
                .FirstOrDefaultAsync(c => c.Id == characterId);

            if (character == null)
                return NotFound("Postać nie znaleziona.");

            if (character.CurrentHp <= 0)
                return BadRequest("Twoja postać jest nieprzytomna! Musisz odpocząć.");

            var monster = await _context.Monsters.FindAsync(monsterId);
            if (monster == null)
                return NotFound("Potwór nie znaleziony.");

            var result = SimulateBattle(character, monster);

            var battleLog = new BattleLog
            {
                CharacterId = character.Id,
                MonsterName = monster.Name,
                Victory = result.Victory,
                ExperienceGained = result.ExperienceGained,
                GoldGained = result.GoldGained,
                DamageDealt = result.DamageDealt,
                DamageTaken = result.DamageTaken,
                BattleText = result.BattleText,
                FoughtAt = DateTime.UtcNow
            };

            _context.BattleLogs.Add(battleLog);

            character.LastActivity = DateTime.UtcNow;

            bool leveledUp = false;
            if (result.Victory)
            {
                character.Experience += result.ExperienceGained;
                character.Gold += result.GoldGained;

                while (character.Experience >= character.ExperienceToNextLevel)
                {
                    character.Experience -= character.ExperienceToNextLevel;
                    character.Level++;
                    character.StatPoints += 5;
                    character.MaxHp += 10;
                    character.CurrentHp = character.MaxHp;
                    character.MaxMana += 5;
                    character.CurrentMana = character.MaxMana;
                    character.ExperienceToNextLevel = (int)(character.ExperienceToNextLevel * 1.5);
                    leveledUp = true;
                }
            }

            await _context.SaveChangesAsync();

            result.LeveledUp = leveledUp;
            result.Character = DtoMapper.MapToDto(character);

            return Ok(result);
        }

        private BattleResultDto SimulateBattle(Character character, Monster monster)
        {
            int charHp = character.CurrentHp;
            int monsterHp = monster.MaxHp;

            int weaponMinDmg = character.Weapon?.MinDamage ?? 0;
            int weaponMaxDmg = character.Weapon?.MaxDamage ?? 0;
            int armorDef = character.Armor?.Defense ?? 0;

            int totalDamageDealt = 0;
            int totalDamageTaken = 0;

            var battleText = new System.Text.StringBuilder();
            battleText.AppendLine($"⚔️ {character.Name} vs {monster.Name}!");
            battleText.AppendLine($"Twoje HP: {charHp} | HP potwora: {monsterHp}");
            battleText.AppendLine("---");

            int round = 0;
            while (charHp > 0 && monsterHp > 0 && round < 20)
            {
                round++;

                // Atak gracza
                int baseDmg = _random.Next(1, character.Strength + 1);
                int weaponDmg = weaponMaxDmg > 0 ? _random.Next(weaponMinDmg, weaponMaxDmg + 1) : 0;
                int playerDmg = baseDmg + weaponDmg;

                // Szansa na trafienie krytyczne (zależna od szczęścia)
                if (_random.Next(100) < character.Luck)
                {
                    playerDmg = (int)(playerDmg * 1.5);
                    battleText.AppendLine($"Runda {round}: TRAFIENIE KRYTYCZNE! Zadajesz {playerDmg} obrażeń!");
                }
                else
                {
                    battleText.AppendLine($"Runda {round}: Zadajesz {playerDmg} obrażeń.");
                }

                monsterHp -= playerDmg;
                totalDamageDealt += playerDmg;

                if (monsterHp <= 0)
                {
                    battleText.AppendLine($"🏆 {monster.Name} pokonany!");
                    break;
                }

                // Atak potwora
                int monsterDmg = _random.Next(1, monster.Strength + 1);
                int dodgeChance = character.Dexterity;
                if (_random.Next(100) < dodgeChance)
                {
                    battleText.AppendLine($"Runda {round}: Unikasz ataku {monster.Name}!");
                }
                else
                {
                    int reducedDmg = Math.Max(1, monsterDmg - armorDef / 3);
                    charHp -= reducedDmg;
                    totalDamageTaken += reducedDmg;
                    battleText.AppendLine($"Runda {round}: {monster.Name} zadaje ci {reducedDmg} obrażeń. (Twoje HP: {Math.Max(0, charHp)})");
                }
            }

            bool victory = monsterHp <= 0;
            int expGained = 0;
            int goldGained = 0;

            if (victory)
            {
                expGained = monster.ExperienceReward;
                goldGained = _random.Next(monster.GoldRewardMin, monster.GoldRewardMax + 1);
                battleText.AppendLine("---");
                battleText.AppendLine($"💰 Zdobywasz {expGained} doświadczenia i {goldGained} złota!");
            }
            else
            {
                battleText.AppendLine("---");
                battleText.AppendLine($"💀 Zostałeś pokonany przez {monster.Name}!");
                charHp = 1;
            }

            character.CurrentHp = Math.Max(0, charHp);

            return new BattleResultDto
            {
                Victory = victory,
                ExperienceGained = expGained,
                GoldGained = goldGained,
                DamageDealt = totalDamageDealt,
                DamageTaken = totalDamageTaken,
                BattleText = battleText.ToString()
            };
        }

        [HttpGet("{characterId}/log")]
        public async Task<ActionResult<List<BattleLog>>> GetBattleLog(int characterId)
        {
            var logs = await _context.BattleLogs
                .Where(bl => bl.CharacterId == characterId)
                .OrderByDescending(bl => bl.FoughtAt)
                .Take(20)
                .ToListAsync();

            return Ok(logs);
        }
    }
}
