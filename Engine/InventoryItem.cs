using System;

namespace Engine
{
    public class InventoryItem : IComparable
    {
        public Item Item { get; set; }
        public int Quantity { get; set; }

        public InventoryItem(Item item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }

        public int CompareTo(object obj)
        {
            if (obj is InventoryItem ii) return Item.Name.CompareTo(ii.Item.Name);
            else throw new ArgumentException("Incorrect parameter's value.");
        }
    }
}
