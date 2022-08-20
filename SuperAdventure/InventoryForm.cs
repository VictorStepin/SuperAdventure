using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Engine;

namespace SuperAdventure
{
    public partial class InventoryForm : Form
    {
        public event Action SortButtonPressed;

        public InventoryForm()
        {
            InitializeComponent();
        }

        public void UpdateUIInventoryList(List<InventoryItem> inventory)
        {
            dgvInventory.Rows.Clear();
            foreach (var ii in inventory)
            {
                if (ii.Quantity > 0)
                {
                    dgvInventory.Rows.Add(new string[] { ii.Item.Name, ii.Quantity.ToString() });
                }
            }
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            SortButtonPressed();
        }
    }
}
