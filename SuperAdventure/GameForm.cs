using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Engine;

namespace SuperAdventure
{
    public partial class GameForm : Form
    {
        private Player _player;
        private Monster _currentMonster;

        public GameForm()
        {
            InitializeComponent();

            _player = new Player(100, 100, 0, 0, 1);
            _player.AddItemToInventory(World.ItemByID(World.ITEM_ID_RUSTY_SWORD));

            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
        }

        private void MoveTo(Location locationToMove)
        {
            if (!_player.HasRequiredItemToEnterLocation(locationToMove))
            {
                PrintMessage($"Для прохода в \"{locationToMove.Name}\" необходим предмет: {locationToMove.ItemRequiredToEnter.Name}");
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
                        _player.ExperiencePoints += questInLocationToMove.RewardExperiencePoints;
                        _player.Gold += questInLocationToMove.RewardGold;
                        _player.AddItemToInventory(questInLocationToMove.RewardItem);
                        PrintMessage($"Вы получили предмет: {questInLocationToMove.RewardItem}.");

                        _player.RemoveQuestCompletionItems(questInLocationToMove);

                        _player.MarkQuestCompleted(questInLocationToMove);

                        PrintMessage($"Вы выполнили квест: {questInLocationToMove.Name}");
                    }
                }
                else
                {
                    var newPlayerQuest = new PlayerQuest(questInLocationToMove);
                    _player.Quests.Add(newPlayerQuest);
                    PrintMessage($"Вы получили квест: \"{newPlayerQuest.Details.Name}\"");
                }
            }

            if (locationToMove.MonsterLivingHere != null)
            {
                PrintMessage($"Вы видете монстра: {locationToMove.MonsterLivingHere.Name}. У него {locationToMove.MonsterLivingHere.CurrentHitPoints} HP.");

                cbxWeapons.Enabled = true;
                btnUseWeapon.Enabled = true;
                cbxPotions.Enabled = true;
                btnUsePotion.Enabled = true;

                var standardMonster = locationToMove.MonsterLivingHere;
                _currentMonster = new Monster(standardMonster.ID,
                                              standardMonster.Name,
                                              standardMonster.Damage,
                                              standardMonster.CurrentHitPoints,
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
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();

            dgvInventory.Rows.Clear();
            foreach (var ii in _player.Inventory)
            {
                if (ii.Quantity > 0)
                {
                    dgvInventory.Rows.Add(new string[] { ii.Details.Name, ii.Quantity.ToString() });
                }
            }

            dgvQuests.Rows.Clear();
            foreach (var pq in _player.Quests)
            {
                dgvQuests.Rows.Add(new string[] { pq.Details.Name, pq.Details.Description, pq.IsCompleted.ToString() });
            }

            rtbLocationInfo.Text = $"{_player.CurrentLocation.Name.ToUpper()}\n{_player.CurrentLocation.Description}";

            btnNorth.Enabled = _player.CurrentLocation.LocationToNorth != null;
            btnEast.Enabled = _player.CurrentLocation.LocationToEast != null;
            btnSouth.Enabled = _player.CurrentLocation.LocationToSouth != null;
            btnWest.Enabled = _player.CurrentLocation.LocationToWest != null;

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

        private void PrintMessage(string message)
        {
            rtbMessages.Text += message + Environment.NewLine;
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

            if (currentWeapon == null)
            {
                PrintMessage("Выберите оружие.");
                return;
            }

            var damage = RNG.NumberBetween(currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);
            _currentMonster.CurrentHitPoints -= damage;

            if (_currentMonster.CurrentHitPoints > 0)
            {
                PrintMessage($"Вы нанесли монстру {damage} урона. У монстра сейчас {_currentMonster.CurrentHitPoints} HP.");

                _player.CurrentHitPoints -= _currentMonster.Damage;
                PrintMessage($"Монстр нанес вам {_currentMonster.Damage} урона.");

                if (_player.CurrentHitPoints <= 0)
                {
                    PrintMessage("Вы умерли.");
                    MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
                    return;
                }
            }
            else
            {
                PrintMessage($"Вы нанесли монстру {damage} урона. Монстр убит.");
                
                _player.Gold += _currentMonster.RewardGold;
                PrintMessage($"Вы получили {_currentMonster.RewardGold} золота.");

                _player.ExperiencePoints += _currentMonster.RewardExperiencePoints;
                PrintMessage($"Вы получили {_currentMonster.RewardExperiencePoints} опыта.");

                foreach (var lootItem in _currentMonster.LootTable)
                {
                    _player.AddItemToInventory(lootItem.Details);
                    PrintMessage($"Вы получили предмет: {lootItem.Details}.");
                }

                MoveTo(_player.CurrentLocation);
            }

            UpdateUI();
        }

        private void btnUsePotion_Click(object sender, System.EventArgs e)
        {
            var currentPotion = (HealingPotion)cbxPotions.SelectedItem;

            if (currentPotion == null)
            {
                PrintMessage("Выберите зелье.");
                return;
            }

            _player.CurrentHitPoints += currentPotion.AmountToHeal;
            if (_player.CurrentHitPoints > _player.MaximumHitPoints)
                _player.CurrentHitPoints = _player.MaximumHitPoints;

            PrintMessage($"Вы использовали {currentPotion.Name}: +{currentPotion.AmountToHeal}HP.");

            UpdateUI();
        }
    }
}
