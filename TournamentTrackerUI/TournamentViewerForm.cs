using TournamentTrackerLibrary;
using TournamentTrackerLibrary.Models;
using TournamentTrackerLibrary.Utility;

namespace TournamentTrackerUI;

public partial class TournamentViewerForm : Form
{
    private readonly TournamentModel tournament;

    public TournamentViewerForm(TournamentModel tournament)
    {
        InitializeComponent();

        // Set up
        this.tournament = tournament;
        WireUpFormData();

        // Event handlers
        matchupListBox.SelectedIndexChanged += MatchupListBox_SelectedIndexChanged;
        roundComboBox.SelectedIndexChanged += RoundComboBox_SelectedIndexChanged;
        scoreButton.Click += ScoreButton_Click;
        unplayedOnlyCheckBox.CheckedChanged += UnplayedOnlyCheckBox_CheckedChanged;
        this.tournament.TournamentCompleted += Tournament_TournamentCompleted;
    }

    private void Tournament_TournamentCompleted(object? sender, DateTime e)
    {
        MessageBox.Show(
            "The tournament is finished!\nCongratulations to the winner!\nHard luck for the others!"
            , "Tournament End"
            , MessageBoxButtons.OK
            , MessageBoxIcon.Information);

        new TournamentDashboardForm().Show();

        this.Close();
    }

    private void DrawMatchupControls()
    {
        bool scoresEnabled = tournament.CurrentRound 
            == ((Round)roundComboBox.SelectedItem).Matchups.First().MatchupRound;

        if (teamOneNameLabel.Text.Length == 0)
        {
            DrawScores(visible: false, scoresEnabled);
        }
        else if (teamTwoNameLabel.Text.Length == 0)
        {
            DrawScores(visible: false, scoresEnabled);
        }
        else if (matchupListBox.SelectedItem == null)
        {
            DrawScores(visible: false, scoresEnabled);
        }
        else
        {
            DrawScores(visible: true, scoresEnabled);
        }
    }

    private void DrawScores(bool visible, bool scoresEanabled)
    {
        //teamOneScoreTextBox.Text = "";
        //teamTwoScoreTextBox.Text = "";
        //teamOneScoreTextBox.Enabled = false;
        //teamTwoScoreTextBox.Enabled = false;
        //scoreButton.Enabled = false;

        teamOneNameLabel.Visible = visible;
        teamTwoNameLabel.Visible = visible;
        scoreButton.Visible = visible;
        teamOneScoreTextBox.Visible = visible;
        teamOneScoreTextBox.Enabled = scoresEanabled;
        teamTwoScoreTextBox.Visible = visible;
        teamTwoScoreTextBox.Enabled = scoresEanabled;
        versusLabel.Visible = visible;
        teamOneScoreLabel.Visible = visible;
        teamTwoScoreLabel.Visible = visible;
    }

    private void MatchupListBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        WireUpMatchupData();
        DrawMatchupControls();
    }

    private void RoundComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        WireUpMatchupList();
    }

    private void ScoreButton_Click(object? sender, EventArgs e)
    {
        if (!ValidateScores())
        {
            return;
        }

        var currentMatchup = (MatchupModel)matchupListBox.SelectedItem;
        var teamOneScoreValue = double.Parse(teamOneScoreTextBox.Text);
        var teamTwoScoreValue = double.Parse(teamTwoScoreTextBox.Text);

        if (currentMatchup == null)
        {
            return;
        }

        TournamentLogic.UpdateMatchupResult(tournament, currentMatchup
            , teamOneScoreValue, teamTwoScoreValue);

        WireUpMatchupList();
    }

    private void UnplayedOnlyCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        WireUpMatchupList();
    }

    private bool ValidateScores()
    {
        bool output = true;

        // TODO - Show appropriate error messages on labels for each case.

        bool teamOneScoreValid = double.TryParse(teamOneScoreTextBox.Text, out double teamOneScoreValue) 
            && teamOneScoreValue >= 0;
        bool teamTwoScoreValid = double.TryParse(teamTwoScoreTextBox.Text, out double teamTwoScoreValue)
            && teamTwoScoreValue >= 0;

        if (!teamOneScoreValid)
        {
            output = false;
        }

        if (!teamTwoScoreValid)
        {
            output = false;
        }

        // Verify abscense of a tie.
        if (teamOneScoreValid && teamTwoScoreValid)
        {
            // Cannot have a tie in the tournament.
            if (teamOneScoreValue == teamTwoScoreValue)
            {
                output = false;
            }
        }

        return output;
    }

    private void WireUpFormData()
    {
        tournamentNameLabel.Text = tournament.TournamentName;

        WireUpRoundsList();

        WireUpMatchupList();

        DrawMatchupControls();
    }

    private void WireUpMatchupData()
    {
        var matchup = (MatchupModel)matchupListBox.SelectedItem;

        if (null != matchup)
        {
            MatchupEntryModel? firstEntry = matchup.Entries.FirstOrDefault();
            MatchupEntryModel? secondEntry = matchup.Entries.ElementAtOrDefault(1);

            teamOneNameLabel.Text = firstEntry?.TeamCompeting?.TeamName;
            teamOneScoreTextBox.Text = firstEntry?.Score.ToString();

            teamTwoNameLabel.Text = secondEntry?.TeamCompeting?.TeamName;
            teamTwoScoreTextBox.Text = secondEntry?.Score.ToString();
        }
    }

    private void WireUpMatchupList()
    {
        var round = (Round)roundComboBox.SelectedItem;

        if (null != round)
        {
            matchupListBox.DataSource = null;

            matchupListBox.DataSource = 
                (unplayedOnlyCheckBox.Checked 
                ? round.Matchups.Where(m => m.Winner == null).ToList()
                : round.Matchups);
                
            matchupListBox.DisplayMember = "MatchupSummary";
        }

        WireUpMatchupData();
    }

    private void WireUpRoundsList()
    {
        roundComboBox.DataSource = null;

        roundComboBox.DataSource = tournament.Rounds;
    }
}
