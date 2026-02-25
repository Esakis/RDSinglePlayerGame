using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedDragonAPI.Data;
using RedDragonAPI.Models;
using RedDragonAPI.Helpers;

namespace RedDragonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("items")]
        public async Task<ActionResult<List<ItemDto>>> GetShopItems()
        {
            var items = await _context.Items
                .Where(i => i.BuyPrice > 0)
                .OrderBy(i => i.Type)
                .ThenBy(i => i.RequiredLevel)
                .ToListAsync();

            return Ok(items.Select(i => DtoMapper.MapItemToDto(i)).ToList());
        }

        [HttpPost("{characterId}/buy/{itemId}")]
        public async Task<ActionResult<CharacterDto>> BuyItem(int characterId, int itemId)
        {
            var character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Armor)
                .FirstOrDefaultAsync(c => c.Id == characterId);

            if (character == null)
                return NotFound("Postać nie znaleziona.");

            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
                return NotFound("Przedmiot nie znaleziony.");

            if (character.Gold < item.BuyPrice)
                return BadRequest("Nie masz wystarczająco złota.");

            if (character.Level < item.RequiredLevel)
                return BadRequest($"Wymagany poziom: {item.RequiredLevel}.");

            character.Gold -= item.BuyPrice;

            var existingItem = await _context.CharacterItems
                .FirstOrDefaultAsync(ci => ci.CharacterId == characterId && ci.ItemId == itemId);

            if (existingItem != null && item.Type == "Potion")
            {
                existingItem.Quantity++;
            }
            else
            {
                _context.CharacterItems.Add(new CharacterItem
                {
                    CharacterId = characterId,
                    ItemId = itemId,
                    Quantity = 1
                });
            }

            await _context.SaveChangesAsync();

            return Ok(DtoMapper.MapToDto(character));
        }

        [HttpPost("{characterId}/sell/{characterItemId}")]
        public async Task<ActionResult<CharacterDto>> SellItem(int characterId, int characterItemId)
        {
            var character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Armor)
                .FirstOrDefaultAsync(c => c.Id == characterId);

            if (character == null)
                return NotFound("Postać nie znaleziona.");

            var charItem = await _context.CharacterItems
                .Include(ci => ci.Item)
                .FirstOrDefaultAsync(ci => ci.Id == characterItemId && ci.CharacterId == characterId);

            if (charItem == null)
                return BadRequest("Nie posiadasz tego przedmiotu.");

            if (character.WeaponId == charItem.ItemId)
                character.WeaponId = null;
            if (character.ArmorId == charItem.ItemId)
                character.ArmorId = null;

            character.Gold += charItem.Item.SellPrice;

            charItem.Quantity--;
            if (charItem.Quantity <= 0)
                _context.CharacterItems.Remove(charItem);

            await _context.SaveChangesAsync();

            return Ok(DtoMapper.MapToDto(character));
        }
    }
}
