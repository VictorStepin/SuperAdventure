using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Engine;

namespace SuperAdventure
{
    public partial class GameForm : Form
    {
        private string _message = "";

        private Player _player;
        private Monster _currentMonster;

        private QuestLogForm questLogForm;
        private InventoryForm inventoryForm;

        public GameForm()
        {
            InitializeComponent();
            inventoryForm = new InventoryForm();
            _player = new Player(World.PlayerMaximumHitPoints,
                                 World.PlayerStartigGold,
                                 World.PlayerStargingExperiencePoints,
                                 World.PlayerStartingLevel);
            _player.AddItemToInventory(World.ItemByID(World.ITEM_ID_RUSTY_SWORD));

            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            
            questLogForm = new QuestLogForm();
            
        }

        private void MoveTo(Location locationToMove)
        {
            if (!_player.HasRequiredItemToEnterLocation(locationToMove))
            {
                AppendMessage($"You need: {locationToMove.ItemRequiredToEnter.Name} to access location \"{locationToMove.Name}\" ");
                UpdateUI();
                return;
            }

            _player.CurrentLocation = locationToMove;

            _player.CurrentHitPoints = _player.MaximumHitPoints;

            if (locationToMove.QuestAvailableHere != null)
            {
                var questInLocationToMove = locationToMove.QuestAvailableHere;

                if (_player.HasQuest(questInLocationToMove))
                {
                    if (_player.HasAllCompletionItems(questInLocationToMove))
                    {
                        _player.RemoveQuestCompletionItems(questInLocationToMove);

                        _player.ExperiencePoints += questInLocationToMove.RewardExperiencePoints;
                        _player.Gold += questInLocationToMove.RewardGold;
                        _player.AddItemToInventory(questInLocationToMove.RewardItem);

                        _player.MarkQuestCompleted(questInLocationToMove);
                        
                        AppendMessage($"You've completed the quest: {questInLocationToMove.Name}");
                        AppendMessage($"You've got the item: {questInLocationToMove.RewardItem.Name}.");
                    }
                }
                else
                {
                    var newPlayerQuest = new PlayerQuest(questInLocationToMove);
                    _player.Quests.Add(newPlayerQuest);
                    AppendMessage($"You've got the quest: \"{newPlayerQuest.Details.Name}\"");
                }
            }

            if (locationToMove.MonsterLivingHere != null)
            {
                AppendMessage($"You are seeing the monster: {locationToMove.MonsterLivingHere.Name}. It has {locationToMove.MonsterLivingHere.CurrentHitPoints} HP.");

                cbxWeapons.Enabled = true;
                btnUseWeapon.Enabled = true;
                cbxPotions.Enabled = true;
                btnUsePotion.Enabled = true;

                var standardMonster = locationToMove.MonsterLivingHere;
                _currentMonster = new Monster(standardMonster.ID,
                                              standardMonster.Name,
                                              standardMonster.Damage,
                                              standardMonster.MaximumHitPoints,
                                              standardMonster.RewardExperiencePoints,
                                              standardMonster.RewardGold);
                foreach (var lootItem in standardMonster.LootTable)
                {
                    _currentMonster.LootTable.Add(lootItem);
                }
            }
            else
            {
                cbxWeapons.Enabled = false;
                btnUseWeapon.Enabled = false;
                cbxPotions.Enabled = false;
                btnUsePotion.Enabled = false;

                _currentMonster = null;
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            UpdateUIPlayerInfo();
            UpdateUILocationInfo();
            PrintMessage();
            UpdateUIInventoryList();
            UpdateUIQuestList();
            UpdateNavigationButtonsAccessibility();
            UpdateWeaponsAccessibility();
            UpdatePotionsAccessibility();
        }

        private void UpdateUILocationInfo()
        {
            rtbLocationInfo.Text = $"{_player.CurrentLocation.Name.ToUpper()}\n{_player.CurrentLocation.Description}";
        }

        private void UpdateUIPlayerInfo()
        {
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
        }

        private void UpdateUIInventoryList()
        {
            if (inventoryForm != null)
                inventoryForm.UpdateUIInventoryList(_player.Inventory);
        }
        
        private void UpdateUIQuestList()
        {
            if (questLogForm != null)
                questLogForm.UpdateUIQuestList(_player.Quests);
        }
        
        private void UpdateWeaponsAccessibility()
        {
            var weapons = new List<Weapon>();
            foreach (var ii in _player.Inventory)
            {
                if (ii.Details is Weapon && ii.Quantity > 0)
                {
                    weapons.Add((Weapon)ii.Details);
                }
            }

            if (weapons.Count == 0)
            {
                cbxWeapons.Enabled = false;
                btnUseWeapon.Enabled = false;
            }
            else
            {
                cbxWeapons.DataSource = weapons;
                cbxWeapons.DisplayMember = "Name";
                cbxWeapons.ValueMember = "ID";
                cbxWeapons.SelectedIndex = 0;
            }
        }
        
        private void UpdatePotionsAccessibility()
        {
            var potions = new List<HealingPotion>();
            foreach (var ii in _player.Inventory)
            {
                if (ii.Details is HealingPotion && ii.Quantity > 0)
                {
                    potions.Add((HealingPotion)ii.Details);
                }
            }

            if (potions.Count == 0)
            {
                cbxPotions.Enabled = false;
                btnUsePotion.Enabled = false;
            }
            else
            {
                cbxPotions.DataSource = potions;
                cbxPotions.DisplayMember = "Name";
                cbxPotions.ValueMember = "ID";
                cbxPotions.SelectedIndex = 0;
            }
        }
        
        private void UpdateNavigationButtonsAccessibility()
        {
            btnNorth.Enabled = _player.CurrentLocation.LocationToNorth != null;
            btnEast.Enabled = _player.CurrentLocation.LocationToEast != null;
            btnSouth.Enabled = _player.CurrentLocation.LocationToSouth != null;
            btnWest.Enabled = _player.CurrentLocation.LocationToWest != null;
        }

        private void AppendMessage(string appendText)
        {
            _message += appendText + Environment.NewLine;
        }

        private void PrintMessage()
        {
            if (_message != "")
                rtbMessages.Text += _message + Environment.NewLine;
            _message = "";
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void btnNorth_Click(object sender, System.EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToNorth);
        }

        private void btnEast_Click(object sender, System.EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToEast);
        }

        private void btnSouth_Click(object sender, System.EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToSouth);
        }

        private void btnWest_Click(object sender, System.EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToWest);
        }

        private void btnUseWeapon_Click(object sender, System.EventArgs e)
        {
            var currentWeapon = (Weapon)cbxWeapons.SelectedItem;

            var damage = RNG.NumberBetween(currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);
            _currentMonster.CurrentHitPoints -= damage;

            if (_currentMonster.CurrentHitPoints > 0)
            {
                AppendMessage($"You've hit the monster by {damage} damage. It has {_currentMonster.CurrentHitPoints} HP now.");

                _player.CurrentHitPoints -= _currentMonster.Damage;
                AppendMessage($"Monster's hit you by {_currentMonster.Damage} damage.");

                if (_player.CurrentHitPoints <= 0)
                {
                    AppendMessage("You died.");
                    MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
                    return;
                }
            }
            else
            {
                AppendMessage($"You've hit the monster by {damage} damage. The monster is dead.");
                
                _player.Gold += _currentMonster.RewardGold;
                AppendMessage($"You've got {_currentMonster.RewardGold} gold.");

                _player.ExperiencePoints += _currentMonster.RewardExperiencePoints;
                AppendMessage($"You've got {_currentMonster.RewardExperiencePoints} experience points.");

                foreach (var lootItem in _currentMonster.LootTable)
                {
                    _player.AddItemToInventory(lootItem.Details);
                    AppendMessage($"You've got the item: {lootItem.Details.Name}.");
                }

                MoveTo(_player.CurrentLocation);
            }

            UpdateUI();
        }

        private void btnUsePotion_Click(object sender, System.EventArgs e)
        {
            var currentPotion = (HealingPotion)cbxPotions.SelectedItem;

            _player.CurrentHitPoints += currentPotion.AmountToHeal;
            if (_player.CurrentHitPoints > _player.MaximumHitPoints)
                _player.CurrentHitPoints = _player.MaximumHitPoints;

            foreach (var ii in _player.Inventory)
            {
                if (ii.Details.ID == currentPotion.ID)
                {
                    ii.Quantity--;
                    break;
                }
            }

            AppendMessage($"You've used {currentPotion.Name}: +{currentPotion.AmountToHeal}HP.");

            UpdateUI();
        }

        private void btnQuestLog_Click(object sender, EventArgs e)
        {
            questLogForm.Show();
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            inventoryForm.Show();
        }
    }
}
