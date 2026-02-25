namespace RedDragonAPI.Models
{
    public class Monster
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int Level { get; set; } = 1;
        public int MaxHp { get; set; } = 50;
        public int Strength { get; set; } = 5;
        public int Dexterity { get; set; } = 5;
        public int Endurance { get; set; } = 5;

        public int ExperienceReward { get; set; } = 20;
        public int GoldRewardMin { get; set; } = 5;
        public int GoldRewardMax { get; set; } = 15;

        public string? LootTable { get; set; }
    }
}
