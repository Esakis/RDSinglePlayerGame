namespace RedDragonAPI.Models
{
    public class BattleLog
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; } = null!;
        public string MonsterName { get; set; } = string.Empty;
        public bool Victory { get; set; }
        public int ExperienceGained { get; set; }
        public int GoldGained { get; set; }
        public int DamageDealt { get; set; }
        public int DamageTaken { get; set; }
        public string BattleText { get; set; } = string.Empty;
        public DateTime FoughtAt { get; set; } = DateTime.UtcNow;
    }
}
