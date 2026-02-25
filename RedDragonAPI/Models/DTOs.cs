namespace RedDragonAPI.Models
{
    public class RegisterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AllocateStatDto
    {
        public string Stat { get; set; } = string.Empty;
        public int Points { get; set; } = 1;
    }

    public class CharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Race { get; set; } = string.Empty;
        public int Level { get; set; }
        public int Experience { get; set; }
        public int ExperienceToNextLevel { get; set; }
        public int MaxHp { get; set; }
        public int CurrentHp { get; set; }
        public int MaxMana { get; set; }
        public int CurrentMana { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int Endurance { get; set; }
        public int Luck { get; set; }
        public int StatPoints { get; set; }
        public int Gold { get; set; }
        public ItemDto? Weapon { get; set; }
        public ItemDto? Armor { get; set; }
    }

    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Value { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public int BonusStrength { get; set; }
        public int BonusDexterity { get; set; }
        public int BonusIntelligence { get; set; }
        public int BonusEndurance { get; set; }
        public int BonusHp { get; set; }
        public int BonusMana { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int Defense { get; set; }
        public int RequiredLevel { get; set; }
    }

    public class InventoryItemDto
    {
        public int CharacterItemId { get; set; }
        public ItemDto Item { get; set; } = null!;
        public int Quantity { get; set; }
    }

    public class BattleResultDto
    {
        public bool Victory { get; set; }
        public int ExperienceGained { get; set; }
        public int GoldGained { get; set; }
        public int DamageDealt { get; set; }
        public int DamageTaken { get; set; }
        public string BattleText { get; set; } = string.Empty;
        public bool LeveledUp { get; set; }
        public CharacterDto Character { get; set; } = null!;
    }

    public class MonsterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Level { get; set; }
        public int MaxHp { get; set; }
        public int ExperienceReward { get; set; }
        public int GoldRewardMin { get; set; }
        public int GoldRewardMax { get; set; }
    }
}
