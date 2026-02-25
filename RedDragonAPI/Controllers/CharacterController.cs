using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedDragonAPI.Data;
using RedDragonAPI.Models;
using RedDragonAPI.Helpers;

namespace RedDragonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CharacterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterDto>> GetCharacter(int id)
        {
            var character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Armor)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
                return NotFound("Postać nie znaleziona.");

            return Ok(DtoMapper.MapToDto(character));
        }

        [HttpPost("{id}/allocate-stat")]
        public async Task<ActionResult<CharacterDto>> AllocateStat(int id, [FromBody] AllocateStatDto dto)
        {
            var character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Armor)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
                return NotFound("Postać nie znaleziona.");

            if (character.StatPoints < dto.Points)
                return BadRequest("Nie masz wystarczająco punktów statystyk.");

            if (dto.Points < 1)
                return BadRequest("Musisz przydzielić co najmniej 1 punkt.");

            switch (dto.Stat.ToLower())
            {
                case "strength":
                case "siła":
                    character.Strength += dto.Points;
                    break;
                case "dexterity":
                case "zręczność":
                    character.Dexterity += dto.Points;
                    break;
                case "intelligence":
                case "inteligencja":
                    character.Intelligence += dto.Points;
                    break;
                case "endurance":
                case "wytrzymałość":
                    character.Endurance += dto.Points;
                    character.MaxHp += dto.Points * 5;
                    character.CurrentHp += dto.Points * 5;
                    break;
                case "luck":
                case "szczęście":
                    character.Luck += dto.Points;
                    break;
                default:
                    return BadRequest("Nieznana statystyka.");
            }

            character.StatPoints -= dto.Points;
            await _context.SaveChangesAsync();

            return Ok(DtoMapper.MapToDto(character));
        }

        [HttpPost("{id}/rest")]
        public async Task<ActionResult<CharacterDto>> Rest(int id)
        {
            var character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Armor)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
                return NotFound("Postać nie znaleziona.");

            character.CurrentHp = character.MaxHp;
            character.CurrentMana = character.MaxMana;
            character.LastActivity = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(DtoMapper.MapToDto(character));
        }

        [HttpGet("{id}/inventory")]
        public async Task<ActionResult<List<InventoryItemDto>>> GetInventory(int id)
        {
            var charItems = await _context.CharacterItems
                .Where(ci => ci.CharacterId == id)
                .Include(ci => ci.Item)
                .ToListAsync();

            var items = charItems.Select(ci => new InventoryItemDto
            {
                CharacterItemId = ci.Id,
                Item = DtoMapper.MapItemToDto(ci.Item),
                Quantity = ci.Quantity
            }).ToList();

            return Ok(items);
        }

        [HttpPost("{id}/equip/{itemId}")]
        public async Task<ActionResult<CharacterDto>> EquipItem(int id, int itemId)
        {
            var character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Armor)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
                return NotFound("Postać nie znaleziona.");

            var charItem = await _context.CharacterItems
                .Include(ci => ci.Item)
                .FirstOrDefaultAsync(ci => ci.CharacterId == id && ci.ItemId == itemId);

            if (charItem == null)
                return BadRequest("Nie posiadasz tego przedmiotu.");

            var item = charItem.Item;

            if (item.RequiredLevel > character.Level)
                return BadRequest($"Wymagany poziom: {item.RequiredLevel}.");

            if (item.Type == "Weapon")
                character.WeaponId = item.Id;
            else if (item.Type == "Armor")
                character.ArmorId = item.Id;
            else
                return BadRequest("Ten przedmiot nie może być założony.");

            await _context.SaveChangesAsync();

            return Ok(DtoMapper.MapToDto(character));
        }

        [HttpPost("{id}/use-potion/{itemId}")]
        public async Task<ActionResult<CharacterDto>> UsePotion(int id, int itemId)
        {
            var character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Armor)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
                return NotFound("Postać nie znaleziona.");

            var charItem = await _context.CharacterItems
                .Include(ci => ci.Item)
                .FirstOrDefaultAsync(ci => ci.CharacterId == id && ci.ItemId == itemId);

            if (charItem == null)
                return BadRequest("Nie posiadasz tego przedmiotu.");

            var item = charItem.Item;

            if (item.Type != "Potion")
                return BadRequest("To nie jest mikstura.");

            if (item.BonusHp > 0)
            {
                character.CurrentHp = Math.Min(character.MaxHp, character.CurrentHp + item.BonusHp);
            }
            if (item.BonusMana > 0)
            {
                character.CurrentMana = Math.Min(character.MaxMana, character.CurrentMana + item.BonusMana);
            }

            charItem.Quantity--;
            if (charItem.Quantity <= 0)
                _context.CharacterItems.Remove(charItem);

            await _context.SaveChangesAsync();

            return Ok(DtoMapper.MapToDto(character));
        }

        [HttpGet("ranking")]
        public async Task<ActionResult<List<CharacterDto>>> GetRanking()
        {
            var characters = await _context.Characters
                .OrderByDescending(c => c.Level)
                .ThenByDescending(c => c.Experience)
                .Take(20)
                .Select(c => new CharacterDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Race = c.Race,
                    Level = c.Level,
                    Experience = c.Experience
                })
                .ToListAsync();

            return Ok(characters);
        }
    }
}
