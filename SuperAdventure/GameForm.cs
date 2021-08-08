using System;
using System.Windows.Forms;

namespace SuperAdventure
{
    public partial class GameForm : Form
    {
        public GameForm()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            lblHitPoints.Text = "100";
            lblGold.Text = "123";
        }
    }
}
