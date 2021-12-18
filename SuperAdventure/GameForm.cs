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

            _player = new Player();

            _player.MaximumHitPoints = 100;
            _player.CurrentHitPoints = _player.MaximumHitPoints;
            _player.Gold = 1073;
            _player.ExperiencePoints = 0;
            _player.Level = 1;

            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
        }
    }
}
