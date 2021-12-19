namespace Engine
{
    public class Monster : LivingCreature
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int RewardExperiencePoints { get; set; }
        public int RewardGold { get; set; }

        public Monster(int id, string name, int currentHitPoints, int maximumHitPoints, int rewardExperiencePoints, int rewardGold)
            : base(currentHitPoints, maximumHitPoints)
        {
            ID = id;
            Name = name;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
        }
    }
}
