using TournamentTrackerLibrary.DataAccess.TextFileHelpers;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess;

public class TextFileConnector : DataConnectorBase
{
    // TODO - (OPTIONAL) Take a look at AutoMapper, useful to learn

    public override void CreatePerson(PersonModel person)
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
        persons.SaveToModelFile(GlobalConfig.PersonFile);
    }

    public override void CreatePrize(PrizeModel prize)
    {
        // Load text file contents and convert them to List<PrizeModel>
        var prizes = GlobalConfig.PrizeFile.FullFilePath().LoadFile().ConvertToPrizeModels();

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
        prizes.SaveToModelFile(GlobalConfig.TeamFile);
    }

    public override void CreateTeam(TeamModel team)  
    {
        // Load all teams from teams text file
        var teams = GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels();

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
        teams.SaveToTeamFile();
    }

    public override List<PersonModel> GetPerson_All()
    {
        return GlobalConfig.PersonFile.FullFilePath().LoadFile().ConvertToPersonModels();
    }

    public override List<TeamModel> GetTeam_All()
    {
        return GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels();
    }

    public override List<TournamentModel> GetTournament_All()
    {
        return
            GlobalConfig.TournamentFile
            .FullFilePath()
            .LoadFile()
            .ConvertToTournamentModels();
    }

    public override void UpdateMatchup(MatchupModel matchup)
    {
        matchup.UpdateMatchup();
    }

    protected override void CreateTournamentImpl(TournamentModel tournament)
    {
        var tournaments =
            GlobalConfig.TournamentFile
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

        tournaments.SaveToTournamentFile();
    }
}
