namespace RedDragonAPI.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Race { get; set; } = "Człowiek";

        public int Level { get; set; } = 1;
        public int Experience { get; set; } = 0;
        public int ExperienceToNextLevel { get; set; } = 100;

        public int MaxHp { get; set; } = 100;
        public int CurrentHp { get; set; } = 100;
        public int MaxMana { get; set; } = 50;
        public int CurrentMana { get; set; } = 50;

        public int Strength { get; set; } = 10;
        public int Dexterity { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public int Endurance { get; set; } = 10;
        public int Luck { get; set; } = 10;

        public int StatPoints { get; set; } = 0;

        public int Gold { get; set; } = 50;

        public int? WeaponId { get; set; }
        public Item? Weapon { get; set; }
        public int? ArmorId { get; set; }
        public Item? Armor { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastActivity { get; set; } = DateTime.UtcNow;

        public ICollection<CharacterItem> Inventory { get; set; } = new List<CharacterItem>();
        public ICollection<BattleLog> BattleLogs { get; set; } = new List<BattleLog>();
    }
}
