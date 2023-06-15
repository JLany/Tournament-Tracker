using TournamentTrackerLibrary;
using TournamentTrackerLibrary.InterCommunication;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerUI
{
    public partial class TournamentDashboardForm : Form, ITournamentRequester
    {
        private readonly List<TournamentModel> tournaments = GlobalConfig.Connector.GetTournament_All();

        public TournamentDashboardForm()
        {
            InitializeComponent();

            // Setup
            WireUpTournamentsList();

            // Event handlers
            createTournamentButton.Click += CreateTournamentButton_Click;
            loadTournamentButton.Click += LoadTournamentButton_Click;
        }

        public void ReceiveTournament(TournamentModel tournament)
        {
            if (null != tournament)
            {
                tournaments.Add(tournament);

                WireUpTournamentsList();
            }
        }

        private void CreateTournamentButton_Click(object? sender, EventArgs e)
        {
            var form = new CreateTournamentForm(this);
            form.Show();
        }

        private void LoadTournamentButton_Click(object? sender, EventArgs e)
        {
            var tournament = (TournamentModel)selectTournamentComboBox.SelectedItem;

            if (null != tournament)
            {
                var form = new TournamentViewerForm(tournament);
                form.Show();

                this.Close();
            }
        }

        private void WireUpTournamentsList()
        {
            selectTournamentComboBox.DataSource = null;

            selectTournamentComboBox.DataSource = tournaments.OrderBy(t => t.TournamentName).ToList();
            selectTournamentComboBox.DisplayMember = "TournamentName";
        }
    }
}
