using TournamentTrackerLibrary.Models;

namespace TournamentTrackerUI
{
    public partial class TournamentViewerForm : Form
    {
        private readonly TournamentModel tournament;

        public TournamentViewerForm(TournamentModel tournament)
        {
            InitializeComponent();

            // Set up
            this.tournament = tournament;
            WireUpFormData();
        }

        private void WireUpFormData()
        {
            tournamentNameLabel.Text = tournament.TournamentName;
        }
    }
}