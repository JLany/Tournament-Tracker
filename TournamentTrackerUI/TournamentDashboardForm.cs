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
            new CreateTournamentForm(this).Show();
        }

        private void LoadTournamentButton_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void WireUpTournamentsList()
        {
            selectTournamentComboBox.DataSource = null;

            selectTournamentComboBox.DataSource = tournaments.OrderBy(t => t.TournamentName).ToList();
            selectTournamentComboBox.DisplayMember = "TournamentName";
        }
    }
}
