using System;
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
            AddItemsToInventory(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1);

            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
        }

        private void MoveTo(Location locationToMove)
        {
            if (locationToMove.ItemRequiredToEnter != null)
            {
                bool playerHasRequiredItem = false;
                foreach (var invenoryItem in _player.Inventory)
                {
                    if (invenoryItem.Details.ID == locationToMove.ItemRequiredToEnter.ID)
                    {
                        playerHasRequiredItem = true;
                        break;
                    }
                }

                if (!playerHasRequiredItem)
                {
                    PrintMessage($"Для прохода в \"{locationToMove.Name}\" необходим предмет: {locationToMove.ItemRequiredToEnter.Name}");
                    return;
                }
            }

            _player.CurrentLocation = locationToMove;

            _player.CurrentHitPoints = _player.MaximumHitPoints;

            if (locationToMove.QuestAvailableHere != null)
            {
                var currentLocationQuest = locationToMove.QuestAvailableHere;
                
                bool playerAlreadyHasQuest = false;
                foreach (var playerQuest in _player.Quests)
                {
                    if (playerQuest.Details.ID == currentLocationQuest.ID)
                    {
                        playerAlreadyHasQuest = true;
                        break;
                    }
                }

                if (playerAlreadyHasQuest)
                {
                    var questCompletionItems = currentLocationQuest.QuestCompletionItems;

                    var playerHasItemsToCompleteQuest = true;
                    foreach (var questCompletionItem in questCompletionItems)
                    {
                        var foundItemInInvenory = false;
                        foreach (var inventoryItem in _player.Inventory)
                        {
                            if (questCompletionItem.Details.ID == inventoryItem.Details.ID)
                            {
                                foundItemInInvenory = true;
                                if (inventoryItem.Quantity < questCompletionItem.Quantity)
                                {
                                    playerHasItemsToCompleteQuest = false;
                                    break;
                                }

                                break;
                            }
                        }

                        if (!foundItemInInvenory)
                        {
                            playerHasItemsToCompleteQuest = false;
                            break;
                        }
                    }

                    if (playerHasItemsToCompleteQuest)
                    {
                        _player.ExperiencePoints += currentLocationQuest.RewardExperiencePoints;
                        _player.Gold += currentLocationQuest.RewardGold;
                        AddItemsToInventory(currentLocationQuest.RewardItem, 1);

                        foreach (var questCompletionItem in questCompletionItems)
                        {
                            foreach (var inventoryItem in _player.Inventory)
                            {
                                if (questCompletionItem.Details.ID == inventoryItem.Details.ID)
                                {
                                    inventoryItem.Quantity -= questCompletionItem.Quantity;
                                    
                                    break;
                                }
                            }
                        }

                        foreach (var playerQuest in _player.Quests)
                        {
                            if (playerQuest.Details.ID == currentLocationQuest.ID)
                            {
                                playerQuest.IsCompleted = true;
                                

                                break;
                            }
                        }

                        PrintMessage($"Вы выполнили квест: {currentLocationQuest.Name}");
                    }
                }
                else
                {
                    var newPlayerQuest = new PlayerQuest(currentLocationQuest);
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
        }

        private void AddItemsToInventory(Item itemToAdd, int quantity)
        {
            var playerAlreadyHasItem = false;
            foreach (var inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Details == itemToAdd)
                {
                    playerAlreadyHasItem = true;
                    inventoryItem.Quantity += quantity;
                    break;
                }
            }

            if (!playerAlreadyHasItem)
            {
                _player.Inventory.Add(new InventoryItem(itemToAdd, quantity));
            }
            
            PrintMessage($"Вы получили предмет: {itemToAdd.Name} x{quantity}.");

            if (itemToAdd is Weapon)
            {
                cbxWeapons.Items.Add(itemToAdd);
            }
            else if (itemToAdd is HealingPotion)
            {
                cbxPotions.Items.Add(itemToAdd);
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
                    AddItemsToInventory(lootItem.Details, 1);
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
