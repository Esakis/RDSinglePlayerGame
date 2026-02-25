using RedDragonAPI.Models;

namespace RedDragonAPI.Helpers
{
    public static class DtoMapper
    {
        public static CharacterDto MapToDto(Character c)
        {
            return new CharacterDto
            {
                Id = c.Id,
                Name = c.Name,
                Race = c.Race,
                Level = c.Level,
                Experience = c.Experience,
                ExperienceToNextLevel = c.ExperienceToNextLevel,
                MaxHp = c.MaxHp,
                CurrentHp = c.CurrentHp,
                MaxMana = c.MaxMana,
                CurrentMana = c.CurrentMana,
                Strength = c.Strength,
                Dexterity = c.Dexterity,
                Intelligence = c.Intelligence,
                Endurance = c.Endurance,
                Luck = c.Luck,
                StatPoints = c.StatPoints,
                Gold = c.Gold,
                Weapon = c.Weapon != null ? MapItemToDto(c.Weapon) : null,
                Armor = c.Armor != null ? MapItemToDto(c.Armor) : null
            };
        }

        public static ItemDto MapItemToDto(Item item)
        {
            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Type = item.Type,
                Value = item.Value,
                BuyPrice = item.BuyPrice,
                SellPrice = item.SellPrice,
                BonusStrength = item.BonusStrength,
                BonusDexterity = item.BonusDexterity,
                BonusIntelligence = item.BonusIntelligence,
                BonusEndurance = item.BonusEndurance,
                BonusHp = item.BonusHp,
                BonusMana = item.BonusMana,
                MinDamage = item.MinDamage,
                MaxDamage = item.MaxDamage,
                Defense = item.Defense,
                RequiredLevel = item.RequiredLevel
            };
        }
    }
}
