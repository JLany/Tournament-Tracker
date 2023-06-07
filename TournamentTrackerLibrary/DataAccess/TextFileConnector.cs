using TournamentTrackerLibrary.DataAccess.TextFileHelpers;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess;

public class TextFileConnector : IDataConnector
{
    // TODO - (OPTIONAL) Take a look at AutoMapper, useful to learn

    private const string PersonFile = "PersonModels.csv";
    private const string PrizeFile = "PrizeModels.csv";
    private const string TeamFile = "TeamModels.csv";
    private const string TournamentFile = "TournamentModels.csv";
    private const string MatchupFile = "MatchupModels.csv";
    private const string MatchupEntryFile = "MatchupEntryModels.csv";

    public void CreatePerson(PersonModel person)
    {
        // Load text file contents and convert them to List<PrizeModel>
        var persons = GetPerson_All();

        // Find the max Id 
        int currentId = 1;

        if (persons.Count > 0)
        {
            currentId = persons.OrderByDescending(p => p.Id).First().Id + 1;
        }

        // Assign the new Id to model
        person.Id = currentId;

        // Add model to prizes list
        persons.Add(person);

        // Convert prizes list to List<string> and save text to the text file
        persons.SaveToModelFile(PersonFile);
    }

    public void CreatePrize(PrizeModel prize)
    {
        // Load text file contents and convert them to List<PrizeModel>
        var prizes = PrizeFile.FullFilePath().LoadFile().ConvertToPrizeModels();

        // Find the max Id 
        int currentId = 1;

        if (prizes.Count > 0)
        {
            currentId = prizes.OrderByDescending(p => p.Id).First().Id + 1;
        }

        // Assign the new Id to model
        prize.Id = currentId;

        // Add model to prizes list
        prizes.Add(prize);

        // Convert prizes list to List<string> and save text to the text file
        prizes.SaveToModelFile(PrizeFile);
    }

    public void CreateTeam(TeamModel team)
    {
        // Load all teams from teams text file
        var teams = TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(PersonFile);

        // Find the max Id
        int currentId = 1;

        if (teams.Count > 0)
        {
            currentId = teams.OrderByDescending(t => t.Id).First().Id + 1;
        }

        // Assign the new Id to our model
        team.Id = currentId;

        // Add model to teams list
        teams.Add(team);

        // Convert to list of string, and save to file
        teams.SaveToTeamFile(TeamFile);
    }

    public void CreateTournament(TournamentModel tournament)
    {
        var tournaments =
            TournamentFile
            .FullFilePath()
            .LoadFile()
            .ConvertToTournamentModels();

        int currentId = 1;

        if (tournaments.Count > 0)
        {
            currentId = tournaments.OrderByDescending(t => t.Id).First().Id + 1;
        }

        tournament.Id = currentId;

        tournament.SaveRounds();

        tournaments.Add(tournament);

        tournaments.SaveToTournamentFile(TournamentFile);
    }

    public List<PersonModel> GetPerson_All()
    {
        return PersonFile.FullFilePath().LoadFile().ConvertToPersonModels();
    }

    public List<TeamModel> GetTeam_All()
    {
        return TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(PersonFile);
    }

    public List<TournamentModel> GetTournament_All()
    {
        return
            GlobalConfig.TournamentFile
            .FullFilePath()
            .LoadFile()
            .ConvertToTournamentModels();
    }

    public void UpdateMatchup(MatchupModel matchup)
    {
        matchup.UpdateMatchup();
    }
}
