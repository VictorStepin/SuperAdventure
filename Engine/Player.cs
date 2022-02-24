using System.Collections.Generic;

namespace Engine
{
    public class Player : LivingCreature
    {
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }
        public Location CurrentLocation { get; private set; }

        public delegate void RequiredItemHandler(string message);
        public event RequiredItemHandler RequiredItemNotify;

        public Player(int currentHitPoints, int maximumHitPoints, int gold, int experiencePoints, int level)
            : base(currentHitPoints, maximumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Level = level;
            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
        }

        public void MoveTo(Location locationToMove)
        {
            if (locationToMove.ItemRequiredToEnter != null)
            {
                bool playerHaveRequiredItem = false;
                foreach (var ii in Inventory)
                {
                    if (ii.Details.ID == locationToMove.ItemRequiredToEnter.ID)
                    {
                        playerHaveRequiredItem = true;
                        break;
                    }
                }

                if (!playerHaveRequiredItem)
                {
                    RequiredItemNotify?.Invoke($"Для прохода в \"{locationToMove.Name}\" необходим предмет: {locationToMove.ItemRequiredToEnter.Name}");
                    return;
                }
            }

            CurrentLocation = locationToMove;

            if (locationToMove.QuestAvailableHere != null)
            {
                bool playerAlreadyHasQuest = false;
                foreach (var pq in Quests)
                {
                    if (pq.Details.ID == locationToMove.QuestAvailableHere.ID)
                    {
                        playerAlreadyHasQuest = true;
                        break;
                    }
                }

                if (!playerAlreadyHasQuest)
                {
                    Quests.Add(new PlayerQuest(locationToMove.QuestAvailableHere));
                    // Событие добавления квеста в грид квестов в интерфейсе
                }
            }
        }
    }
}
