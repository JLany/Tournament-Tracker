using TournamentTrackerLibrary;
using TournamentTrackerLibrary.InterCommunication;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerUI;

public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester
{
    private readonly List<TeamModel> availableTeams = GlobalConfig.Connector.GetTeam_All();
    private readonly List<TeamModel> selectedTeams = new();
    private readonly List<PrizeModel> selectedPrizes = new();
    private readonly ITournamentRequester tournamentRequester;

    public CreateTournamentForm(ITournamentRequester tournamentRequester)
    {
        InitializeComponent();

        // Set up.
        WireUpLists();
        this.tournamentRequester = tournamentRequester;

        // Event handlers.
        addTeamButton.Click += AddTeamButton_Click;
        createNewTeamLink.LinkClicked += CreateNewTeamLink_LinkClicked;
        createPrizeButton.Click += CreatePrizeButton_Click;
        createTournamentButton.Click += CreateTournamentButton_Click;
        removeSelectedPrizeButton.Click += RemoveSelectedPrizeButton_Click;
        removeSelectedTeamButton.Click += RemoveSelectedTeamButton_Click;
    }

    public void ReceivePrize(PrizeModel prize)
    {
        if (null != prize)
        {
            selectedPrizes.Add(prize);
            WireUpLists();
        }
    }

    public void ReceiveTeam(TeamModel team)
    {
        if (null != team)
        {
            selectedTeams.Add(team);
            WireUpLists();
        }
    }

    private void AddTeamButton_Click(object? sender, EventArgs e)
    {
        var team = (TeamModel)selectTeamComboBox.SelectedItem;

        if (null != team)
        {
            availableTeams.Remove(team);
            selectedTeams.Add(team);

            WireUpLists();
        }
    }

    private void CreateNewTeamLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        new CreateTeamForm(this).Show();
    }

    private void CreatePrizeButton_Click(object? sender, EventArgs e)
    {
        new CreatePrizeForm(this).Show();
    }

    private void CreateTournamentButton_Click(object? sender, EventArgs e)
    {
        if (!ValidateTournamentData())
        {
            return;
        }

        var tournament = new TournamentModel
        {
            TournamentName = tournamentNameTextBox.Text,
            EntryFee = decimal.Parse(entryFeeTextBox.Text),
            EnteredTeams = selectedTeams,
            Prizes = selectedPrizes
        };

        GlobalConfig.Connector.CreateTournament(tournament);

        tournamentRequester.ReceiveTournament(tournament);

        this.Close();
    }

    private void RemoveSelectedPrizeButton_Click(object? sender, EventArgs e)
    {
        var prize = (PrizeModel)prizesListBox.SelectedItem;

        if (null != prize)
        {
            selectedPrizes.Remove(prize);

            WireUpLists();
        }
    }

    private void RemoveSelectedTeamButton_Click(object? sender, EventArgs e)
    {
        var team = (TeamModel)selectedTeamsListBox.SelectedItem;

        if (null != team)
        {
            availableTeams.Add(team);
            selectedTeams.Remove(team);

            WireUpLists();
        }
    }

    private void WireUpLists()
    {
        selectTeamComboBox.DataSource = null;
        selectTeamComboBox.DataSource = availableTeams;
        selectTeamComboBox.DisplayMember = "TeamName";

        selectedTeamsListBox.DataSource = null;
        selectedTeamsListBox.DataSource = selectedTeams;
        selectedTeamsListBox.DisplayMember = "TeamName";

        prizesListBox.DataSource = null;
        prizesListBox.DataSource = selectedPrizes.OrderBy(p => p.PlaceNumber).ToList();
        prizesListBox.DisplayMember = "PrizeSummary";
    }

    // TODO - Implement validation for ALL names not to contain a comma (,)!
    private bool ValidateTournamentData()
    {
        // TODO - Implement tournament data validation

        return true;
    }
}
