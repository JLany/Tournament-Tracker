using TournamentTrackerLibrary;
using TournamentTrackerLibrary.Models;

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

    private void LogScores(MatchupModel currentMatchup)
    {
        var teamOneScoreValue = double.Parse(teamOneScoreTextBox.Text);
        var teamTwoScoreValue = double.Parse(teamTwoScoreTextBox.Text);

        // TODO - (OPTIONAL) Try to do better
        //if (teamOneScoreValue > teamTwoScoreValue)
        //{
        //    currentMatchup.Winner = currentMatchup.Entries.ElementAtOrDefault(0)?.TeamCompeting;
        //}
        //else if (teamTwoScoreValue > teamOneScoreValue)
        //{
        //    currentMatchup.Winner = currentMatchup.Entries.ElementAtOrDefault(1)?.TeamCompeting;
        //}
        //else
        //{
        //    throw new InvalidOperationException();
        //}

        double[] scores = new[] { teamOneScoreValue, teamTwoScoreValue };
        for (int team = 0; team < currentMatchup.Entries.Count; team++)
        {
            currentMatchup.Entries.ElementAt(team).Score = scores[team];
        }

        //GlobalConfig.Connector.UpdateMatchup(currentMatchup);
    }

    private void MatchupListBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        WireUpMatchupData();
        DrawMatchupControls();
    }

    private void QualifyWinnerToNextRound()
    {
        var currentMatchup = (MatchupModel)matchupListBox.SelectedItem;

        tournament.Rounds.ForEach(round =>
        {
            foreach (var m in round.Matchups)
            {
                foreach (var entry in m.Entries)
                {
                    if (entry.ParentMatchup?.Id == currentMatchup.Id)
                    {
                        entry.TeamCompeting = currentMatchup.Winner;
                        GlobalConfig.Connector.UpdateMatchup(m);
                        return;
                    }
                }
            }
        });
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

        //LogScores(currentMatchup);

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
        // TODO - Implement scores validation

        // Verify abscense of a tie

        // Validate ...

        return true;
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

            _ = (unplayedOnlyCheckBox.Checked ?
                matchupListBox.DataSource = round.Matchups.Where(m => m.Winner == null).ToList()
                : matchupListBox.DataSource = round.Matchups);

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