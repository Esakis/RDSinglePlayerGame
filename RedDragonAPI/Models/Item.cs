namespace RedDragonAPI.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = "Weapon"; // Weapon, Armor, Potion
        public int Value { get; set; } = 10;
        public int BuyPrice { get; set; } = 0;
        public int SellPrice { get; set; } = 0;

        public int BonusStrength { get; set; } = 0;
        public int BonusDexterity { get; set; } = 0;
        public int BonusIntelligence { get; set; } = 0;
        public int BonusEndurance { get; set; } = 0;
        public int BonusHp { get; set; } = 0;
        public int BonusMana { get; set; } = 0;

        public int MinDamage { get; set; } = 0;
        public int MaxDamage { get; set; } = 0;
        public int Defense { get; set; } = 0;

        public int RequiredLevel { get; set; } = 1;
    }
}
