using System.Collections.Generic;

namespace Engine
{
    public class Player : LivingCreature
    {
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public Location CurrentLocation { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }

        public Player(int maximumHitPoints, int gold, int experiencePoints, int level)
            : base(maximumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Level = level;
            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
        }

        public bool HasRequiredItemToEnterLocation(Location location)
        {
            if (location.ItemRequiredToEnter == null)
                return true;

            foreach (var ii in Inventory)
                if (ii.Item.ID == location.ItemRequiredToEnter.ID)
                    return true;

            return false;
        }

        public bool HasQuest(Quest quest)
        {
            foreach (var pq in Quests)
                if (pq.Details.ID == quest.ID)
                    return true;

            return false;
        }

        public bool CompletedQuest(Quest quest)
        {
            foreach (var pq in Quests)
                if (pq.Details.ID == quest.ID)
                    if (pq.IsCompleted) return true;

            return false;
        }

        public bool HasAllCompletionItems(Quest quest)
        {
            foreach (var qci in quest.QuestCompletionItems)
            {
                var foundItemInInventory = false;
                foreach (var ii in Inventory)
                {
                    if (qci.Details.ID == ii.Item.ID)
                    {
                        foundItemInInventory = true;
                        if (ii.Quantity < qci.Quantity)
                            return false;
                        break;
                    }
                }

                if (!foundItemInInventory)
                    return false;
            }
            
            return true;
        }

        public void RemoveQuestCompletionItems(Quest quest)
        {
            foreach (var qci in quest.QuestCompletionItems)
            {
                foreach (var ii in Inventory)
                {
                    if (qci.Details.ID == ii.Item.ID)
                    {
                        ii.Quantity -= qci.Quantity;
                        break;
                    }
                }
            }
        }

        public void MarkQuestCompleted(Quest quest)
        {
            foreach (var pq in Quests)
            {
                if (pq.Details.ID == quest.ID)
                {
                    pq.IsCompleted = true;
                    return;
                }
            }
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            foreach (var ii in Inventory)
            {
                if (ii.Item.ID == itemToAdd.ID)
                {
                    ii.Quantity++;
                    return;
                }
            }

            Inventory.Add(new InventoryItem(itemToAdd, 1));
        }
    }
}
