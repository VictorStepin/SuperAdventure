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
            _player.RequiredItemNotify += message => rtbMessages.Text += $"{message}\n";
            _player.MoveTo(World.LocationByID(World.LOCATION_ID_HOME));

            UpdateUI();
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

        private void btnNorth_Click(object sender, System.EventArgs e)
        {
            _player.MoveTo(_player.CurrentLocation.LocationToNorth);
            UpdateUI();
        }

        private void btnEast_Click(object sender, System.EventArgs e)
        {
            _player.MoveTo(_player.CurrentLocation.LocationToEast);
            UpdateUI();
        }

        private void btnSouth_Click(object sender, System.EventArgs e)
        {
            _player.MoveTo(_player.CurrentLocation.LocationToSouth);
            UpdateUI();
        }

        private void btnWest_Click(object sender, System.EventArgs e)
        {
            _player.MoveTo(_player.CurrentLocation.LocationToWest);
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
