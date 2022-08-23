using System;

namespace Engine
{
    public class InventoryItem : IComparable
    {
        public Item Details { get; set; }
        public int Quantity { get; set; }

        public InventoryItem(Item details, int quantity)
        {
            Details = details;
            Quantity = quantity;
        }

        public int CompareTo(object obj)
        {
            if (obj is InventoryItem ii) return Details.Name.CompareTo(ii.Details.Name);
            else throw new ArgumentException("Incorrect parameter's value.");
        }
    }
}
