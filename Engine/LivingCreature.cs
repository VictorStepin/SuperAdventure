namespace Engine
{
    public class LivingCreature
    {
        public int CurrentHitPoints { get; set; }
        public int MaximumHitPoints { get; private set; }

        public LivingCreature(int maximumHitPoints)
        {
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = MaximumHitPoints;
        }

        public LivingCreature(int currentHitPoints, int maximumHitPoints)
        {
            CurrentHitPoints = currentHitPoints;
            MaximumHitPoints = maximumHitPoints;
        }
    }
}
