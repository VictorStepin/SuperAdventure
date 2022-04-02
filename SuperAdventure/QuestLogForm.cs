using System.Collections.Generic;
using System.Windows.Forms;
using Engine;

namespace SuperAdventure
{
    public partial class QuestLogForm : Form
    {
        public QuestLogForm()
        {
            InitializeComponent();
        }

        public void UpdateUIQuestList(List<PlayerQuest> quests)
        {
            dgvQuests.Rows.Clear();
            foreach (var pq in quests)
            {
                dgvQuests.Rows.Add(new string[] { pq.Details.Name, pq.Details.Description, pq.IsCompleted.ToString() });
            }
        }

        private void btnHide_Click(object sender, System.EventArgs e)
        {
            Hide();
        }
    }
}
