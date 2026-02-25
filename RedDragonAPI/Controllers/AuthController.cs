using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedDragonAPI.Data;
using RedDragonAPI.Models;
using RedDragonAPI.Helpers;

namespace RedDragonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<CharacterDto>> Register([FromBody] RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Nazwa postaci jest wymagana.");

            if (dto.Name.Length < 3 || dto.Name.Length > 20)
                return BadRequest("Nazwa postaci musi mieć od 3 do 20 znaków.");

            var exists = await _context.Characters.AnyAsync(c => c.Name == dto.Name);
            if (exists)
                return BadRequest("Postać o tej nazwie już istnieje.");

            var character = new Character
            {
                Name = dto.Name,
                Race = "Człowiek",
                Level = 1,
                Experience = 0,
                ExperienceToNextLevel = 100,
                MaxHp = 100,
                CurrentHp = 100,
                MaxMana = 50,
                CurrentMana = 50,
                Strength = 10,
                Dexterity = 10,
                Intelligence = 10,
                Endurance = 10,
                Luck = 10,
                StatPoints = 0,
                Gold = 50,
                CreatedAt = DateTime.UtcNow,
                LastActivity = DateTime.UtcNow
            };

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            var startingWeapon = await _context.Items.FirstOrDefaultAsync(i => i.Id == 1);
            if (startingWeapon != null)
            {
                _context.CharacterItems.Add(new CharacterItem
                {
                    CharacterId = character.Id,
                    ItemId = startingWeapon.Id,
                    Quantity = 1
                });
                await _context.SaveChangesAsync();
            }

            var characterWithItems = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Armor)
                .FirstOrDefaultAsync(c => c.Id == character.Id);

            return Ok(DtoMapper.MapToDto(characterWithItems!));
        }

        [HttpPost("login")]
        public async Task<ActionResult<CharacterDto>> Login([FromBody] LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Nazwa postaci jest wymagana.");

            var character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Armor)
                .FirstOrDefaultAsync(c => c.Name == dto.Name);

            if (character == null)
                return NotFound("Postać nie znaleziona.");

            character.LastActivity = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(DtoMapper.MapToDto(character));
        }

        [HttpPost("guest")]
        public async Task<ActionResult<CharacterDto>> LoginAsGuest()
        {
            var guestName = "Gość";
            var character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Armor)
                .FirstOrDefaultAsync(c => c.Name == guestName);

            if (character == null)
            {
                character = new Character
                {
                    Name = guestName,
                    Race = "Człowiek",
                    Level = 1,
                    Experience = 0,
                    ExperienceToNextLevel = 100,
                    MaxHp = 100,
                    CurrentHp = 100,
                    MaxMana = 50,
                    CurrentMana = 50,
                    Strength = 10,
                    Dexterity = 10,
                    Intelligence = 10,
                    Endurance = 10,
                    Luck = 10,
                    StatPoints = 0,
                    Gold = 50,
                    CreatedAt = DateTime.UtcNow,
                    LastActivity = DateTime.UtcNow
                };

                _context.Characters.Add(character);
                await _context.SaveChangesAsync();

                var startingWeapon = await _context.Items.FirstOrDefaultAsync(i => i.Id == 1);
                if (startingWeapon != null)
                {
                    _context.CharacterItems.Add(new CharacterItem
                    {
                        CharacterId = character.Id,
                        ItemId = startingWeapon.Id,
                        Quantity = 1
                    });
                    await _context.SaveChangesAsync();
                }

                character = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Armor)
                    .FirstOrDefaultAsync(c => c.Id == character.Id);
            }
            else
            {
                character.LastActivity = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return Ok(DtoMapper.MapToDto(character!));
        }
    }
}
