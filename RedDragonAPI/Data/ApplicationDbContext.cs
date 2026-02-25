using Microsoft.EntityFrameworkCore;
using RedDragonAPI.Models;

namespace RedDragonAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Monster> Monsters { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<CharacterItem> CharacterItems { get; set; }
        public DbSet<BattleLog> BattleLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Character>(entity =>
            {
                entity.HasIndex(c => c.Name).IsUnique();

                entity.HasOne(c => c.Weapon)
                    .WithMany()
                    .HasForeignKey(c => c.WeaponId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(c => c.Armor)
                    .WithMany()
                    .HasForeignKey(c => c.ArmorId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<CharacterItem>(entity =>
            {
                entity.HasOne(ci => ci.Character)
                    .WithMany(c => c.Inventory)
                    .HasForeignKey(ci => ci.CharacterId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ci => ci.Item)
                    .WithMany()
                    .HasForeignKey(ci => ci.ItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<BattleLog>(entity =>
            {
                entity.HasOne(bl => bl.Character)
                    .WithMany(c => c.BattleLogs)
                    .HasForeignKey(bl => bl.CharacterId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Monster>().HasData(
                new Monster { Id = 1, Name = "Szczur", Description = "Zwykły szczur z kanałów. Łatwy przeciwnik dla początkujących.", Level = 1, MaxHp = 30, Strength = 3, Dexterity = 5, Endurance = 2, ExperienceReward = 15, GoldRewardMin = 1, GoldRewardMax = 5 },
                new Monster { Id = 2, Name = "Goblin", Description = "Mały zielony stwór uzbrojony w nóż.", Level = 2, MaxHp = 50, Strength = 6, Dexterity = 7, Endurance = 4, ExperienceReward = 30, GoldRewardMin = 5, GoldRewardMax = 15 },
                new Monster { Id = 3, Name = "Wilk", Description = "Dziki wilk grasujący po okolicy.", Level = 3, MaxHp = 70, Strength = 9, Dexterity = 10, Endurance = 6, ExperienceReward = 50, GoldRewardMin = 8, GoldRewardMax = 20 },
                new Monster { Id = 4, Name = "Szkielet", Description = "Ożywiony szkielet uzbrojony w zardzewiały miecz.", Level = 4, MaxHp = 90, Strength = 12, Dexterity = 8, Endurance = 10, ExperienceReward = 75, GoldRewardMin = 12, GoldRewardMax = 30 },
                new Monster { Id = 5, Name = "Ork", Description = "Potężny ork wojownik.", Level = 5, MaxHp = 120, Strength = 15, Dexterity = 9, Endurance = 12, ExperienceReward = 100, GoldRewardMin = 20, GoldRewardMax = 50 },
                new Monster { Id = 6, Name = "Troll", Description = "Ogromny troll z regeneracją.", Level = 7, MaxHp = 180, Strength = 20, Dexterity = 6, Endurance = 18, ExperienceReward = 160, GoldRewardMin = 30, GoldRewardMax = 70 },
                new Monster { Id = 7, Name = "Wampir", Description = "Starożytny wampir wysysający krew.", Level = 9, MaxHp = 200, Strength = 18, Dexterity = 16, Endurance = 14, ExperienceReward = 220, GoldRewardMin = 50, GoldRewardMax = 100 },
                new Monster { Id = 8, Name = "Czerwony Smok", Description = "Potężny czerwony smok ziający ogniem. Legendarny przeciwnik.", Level = 12, MaxHp = 350, Strength = 30, Dexterity = 15, Endurance = 25, ExperienceReward = 500, GoldRewardMin = 100, GoldRewardMax = 250 }
            );

            modelBuilder.Entity<Item>().HasData(
                // Bronie
                new Item { Id = 1, Name = "Drewniany Kij", Description = "Prosty kij z drewna.", Type = "Weapon", BuyPrice = 10, SellPrice = 3, MinDamage = 1, MaxDamage = 4, RequiredLevel = 1 },
                new Item { Id = 2, Name = "Zardzewiały Miecz", Description = "Stary, zardzewiały miecz.", Type = "Weapon", BuyPrice = 30, SellPrice = 10, MinDamage = 3, MaxDamage = 8, RequiredLevel = 2 },
                new Item { Id = 3, Name = "Krótki Miecz", Description = "Lekki i szybki miecz.", Type = "Weapon", BuyPrice = 80, SellPrice = 30, MinDamage = 5, MaxDamage = 12, BonusDexterity = 1, RequiredLevel = 3 },
                new Item { Id = 4, Name = "Miecz Stalowy", Description = "Solidny stalowy miecz.", Type = "Weapon", BuyPrice = 200, SellPrice = 75, MinDamage = 8, MaxDamage = 18, BonusStrength = 2, RequiredLevel = 5 },
                new Item { Id = 5, Name = "Topór Bojowy", Description = "Ciężki topór zadający duże obrażenia.", Type = "Weapon", BuyPrice = 350, SellPrice = 130, MinDamage = 12, MaxDamage = 25, BonusStrength = 3, RequiredLevel = 7 },
                new Item { Id = 6, Name = "Miecz Ognia", Description = "Magiczny miecz płonący ogniem.", Type = "Weapon", BuyPrice = 700, SellPrice = 260, MinDamage = 18, MaxDamage = 35, BonusStrength = 4, BonusIntelligence = 2, RequiredLevel = 10 },

                // Zbroje
                new Item { Id = 7, Name = "Skórzana Kamizelka", Description = "Prosta kamizelka ze skóry.", Type = "Armor", BuyPrice = 15, SellPrice = 5, Defense = 2, RequiredLevel = 1 },
                new Item { Id = 8, Name = "Skórzana Zbroja", Description = "Pełna zbroja ze skóry.", Type = "Armor", BuyPrice = 50, SellPrice = 18, Defense = 5, BonusEndurance = 1, RequiredLevel = 2 },
                new Item { Id = 9, Name = "Kolczuga", Description = "Zbroja z metalowych kółek.", Type = "Armor", BuyPrice = 150, SellPrice = 55, Defense = 10, BonusEndurance = 2, RequiredLevel = 4 },
                new Item { Id = 10, Name = "Zbroja Płytowa", Description = "Ciężka zbroja z metalowych płyt.", Type = "Armor", BuyPrice = 400, SellPrice = 150, Defense = 18, BonusEndurance = 3, RequiredLevel = 6 },
                new Item { Id = 11, Name = "Zbroja Smoka", Description = "Legendarna zbroja z łusek smoka.", Type = "Armor", BuyPrice = 900, SellPrice = 350, Defense = 30, BonusEndurance = 5, BonusStrength = 2, RequiredLevel = 10 },

                // Mikstury
                new Item { Id = 12, Name = "Mała Mikstura Zdrowia", Description = "Przywraca 30 HP.", Type = "Potion", BuyPrice = 10, SellPrice = 3, BonusHp = 30, RequiredLevel = 1 },
                new Item { Id = 13, Name = "Duża Mikstura Zdrowia", Description = "Przywraca 80 HP.", Type = "Potion", BuyPrice = 30, SellPrice = 10, BonusHp = 80, RequiredLevel = 3 },
                new Item { Id = 14, Name = "Mała Mikstura Many", Description = "Przywraca 25 Many.", Type = "Potion", BuyPrice = 15, SellPrice = 5, BonusMana = 25, RequiredLevel = 1 },
                new Item { Id = 15, Name = "Duża Mikstura Many", Description = "Przywraca 60 Many.", Type = "Potion", BuyPrice = 40, SellPrice = 15, BonusMana = 60, RequiredLevel = 3 }
            );
        }
    }
}
