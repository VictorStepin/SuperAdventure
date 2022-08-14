using System.Collections.Generic;

namespace Engine
{
    public class Monster : LivingCreature
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public int RewardExperiencePoints { get; set; }
        public int RewardGold { get; set; }
        public List<LootItem> LootTable { get; set; }

        public Monster(int id,
                       string name,
                       int damage, 
                       int hitPoints, 
                       int rewardExperiencePoints, 
                       int rewardGold)
            : base(hitPoints)
        {
            ID = id;
            Name = name;
            Damage = damage;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            LootTable = new List<LootItem>();
        }
    }
}
