using System.Collections.Generic;
using System.Linq;

namespace Engine
{
    public class Player : LivingCreature
    {
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get => (ExperiencePoints / 100) + 1; }
        public Location CurrentLocation { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }

        public Player(int maximumHitPoints, int gold, int experiencePoints)
            : base(maximumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
        }

        public bool HasRequiredItemToEnterLocation(Location location)
        {
            if (location.ItemRequiredToEnter == null)
                return true;

            return Inventory.Exists(ii => ii.Item.ID == location.ItemRequiredToEnter.ID);
        }

        public bool HasQuest(Quest quest)
            => Quests.Exists(pq => pq.Details.ID == quest.ID);

        public bool CompletedQuest(Quest quest)
            => Quests.Exists(pq => pq.Details.ID == quest.ID && pq.IsCompleted);

        public bool HasAllCompletionItems(Quest quest)
        {
            foreach (var qci in quest.QuestCompletionItems)
                if (!Inventory.Exists(ii => ii.Item.ID == qci.Details.ID && ii.Quantity >= qci.Quantity))
                    return false;

            return true;
        }

        public void RemoveQuestCompletionItems(Quest quest)
        {
            foreach (var qci in quest.QuestCompletionItems)
            {
                var itemToRemove = Inventory.SingleOrDefault(ii => ii.Quantity >= qci.Quantity);

                if (itemToRemove != null) itemToRemove.Quantity -= qci.Quantity;
            }
        }

        public void MarkQuestCompleted(Quest quest)
        {
            var questToMark = Quests.SingleOrDefault(pq => pq.Details.ID == quest.ID);
            
            if (questToMark != null) questToMark.IsCompleted = true;
        }

        public void AddItemToInventory(Item item)
        {
            var availableItem = Inventory.SingleOrDefault(ii => ii.Item.ID == item.ID);
            
            if (availableItem != null) availableItem.Quantity++;
            else Inventory.Add(new InventoryItem(item, 1));
        }
    }
}
