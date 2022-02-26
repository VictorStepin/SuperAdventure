using System;
using System.Windows.Forms;
using Engine;

namespace SuperAdventure
{
    public partial class GameForm : Form
    {
        private Player _player;

        public GameForm()
        {
            InitializeComponent();

            _player = new Player(100, 100, 0, 0, 1);
            AddItemsToInventory(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1);

            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));

            UpdateUI();
        }

        public void MoveTo(Location locationToMove)
        {
            if (locationToMove.ItemRequiredToEnter != null)
            {
                bool playerHaveRequiredItem = false;
                foreach (var invenoryItem in _player.Inventory)
                {
                    if (invenoryItem.Details.ID == locationToMove.ItemRequiredToEnter.ID)
                    {
                        playerHaveRequiredItem = true;
                        break;
                    }
                }

                if (!playerHaveRequiredItem)
                {
                    PrintMessage($"Для прохода в \"{locationToMove.Name}\" необходим предмет: {locationToMove.ItemRequiredToEnter.Name}");
                    return;
                }
            }

            _player.CurrentLocation = locationToMove;

            _player.CurrentHitPoints = _player.MaximumHitPoints;

            if (locationToMove.QuestAvailableHere != null)
            {
                bool playerAlreadyHasQuest = false;
                foreach (var playerQuest in _player.Quests)
                {
                    if (playerQuest.Details.ID == locationToMove.QuestAvailableHere.ID)
                    {
                        playerAlreadyHasQuest = true;
                        break;
                    }
                }

                if (!playerAlreadyHasQuest)
                {
                    var newQuest = new PlayerQuest(locationToMove.QuestAvailableHere);
                    _player.Quests.Add(newQuest);
                    dgvQuests.Rows.Add(new string[] { newQuest.Details.Name, newQuest.Details.Description, newQuest.IsCompleted.ToString() });
                    PrintMessage($"Вы получили квест: \"{newQuest.Details.Name}\"");
                }
                else
                {
                    // Логика для проверки выполнил ли игрок квест или нет
                }
            }

            if (locationToMove.MonsterLivingHere != null)
            {
                PrintMessage($"You see a monster: {locationToMove.MonsterLivingHere.Name}");

                cbxWeapons.Enabled = true;
                btnUseWeapon.Enabled = true;
                cbxPotions.Enabled = true;
                btnUsePotion.Enabled = true;
            }
            else
            {
                cbxWeapons.Enabled = false;
                btnUseWeapon.Enabled = false;
                cbxPotions.Enabled = false;
                btnUsePotion.Enabled = false;
            }
        }

        private void UpdateUI()
        {
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();

            rtbLocationInfo.Text = $"{_player.CurrentLocation.Name.ToUpper()}\n{_player.CurrentLocation.Description}";

            btnNorth.Enabled = _player.CurrentLocation.LocationToNorth != null ? true : false;
            btnEast.Enabled = _player.CurrentLocation.LocationToEast != null ? true : false;
            btnSouth.Enabled = _player.CurrentLocation.LocationToSouth != null ? true : false;
            btnWest.Enabled = _player.CurrentLocation.LocationToWest != null ? true : false;
        }

        private void AddItemsToInventory(Item itemToAdd, int quantity)
        {
            _player.Inventory.Add(new InventoryItem(itemToAdd, quantity));
            dgvInventory.Rows.Add(new string[] { itemToAdd.Name, quantity.ToString() });
            if (itemToAdd is Weapon)
            {
                cbxWeapons.Items.Add(itemToAdd.Name);
            }
        }

        private void PrintMessage(string message)
        {
            rtbMessages.Text += message + Environment.NewLine;
        }

        private void btnNorth_Click(object sender, System.EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToNorth);
            UpdateUI();
        }

        private void btnEast_Click(object sender, System.EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToEast);
            UpdateUI();
        }

        private void btnSouth_Click(object sender, System.EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToSouth);
            UpdateUI();
        }

        private void btnWest_Click(object sender, System.EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToWest);
            UpdateUI();
        }

        private void btnUseWeapon_Click(object sender, System.EventArgs e)
        {
            
        }

        private void btnUsePotion_Click(object sender, System.EventArgs e)
        {

        }
    }
}
