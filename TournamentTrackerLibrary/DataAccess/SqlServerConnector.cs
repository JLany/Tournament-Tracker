using Dapper;
using System.Data;
using System.Reflection;
using TournamentTrackerLibrary.DataAccess.SqlServerConnectorHelper;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess;

public class SqlServerConnector : IDataConnector
{
    private const string DatabaseName = "Tournaments";

    public void CreatePerson(PersonModel person)
    {
        using IDbConnection connection =
            new System.Data.SqlClient.SqlConnection(GlobalConfig.GetConnectionString(DatabaseName));

        var param = new DynamicParameters();
        param.Add("@FirstName", person.FirstName);
        param.Add("@LastName", person.LastName);
        param.Add("@EmailAddress", person.EmailAddress);
        param.Add("@PhoneNumber", person.PhoneNumber);
        param.Add("@Id", person.Id, dbType: DbType.Int32, direction: ParameterDirection.Output);

        connection.Execute("dbo.spPerson_Insert", param, commandType: CommandType.StoredProcedure);

        person.Id = param.Get<int>("@Id");
    }

    public void CreatePrize(PrizeModel prize)
    {
        using IDbConnection connection =
            new System.Data.SqlClient.SqlConnection(GlobalConfig.GetConnectionString(DatabaseName));

        var param = new DynamicParameters();
        param.Add("@PlaceNumber", prize.PlaceNumber);
        param.Add("@PlaceName", prize.PlaceName);
        param.Add("@PrizeAmount", prize.PrizeAmount);
        param.Add("@PrizePercentage", prize.PrizePercentage);

        // send @Id as a parameter to be filled by sql server; more specifically by the stored procedure
        param.Add("@Id", null, dbType: DbType.Int32, direction: ParameterDirection.Output);

        // through the execution of the stored procedure, it will fill @Id
        connection.Execute("dbo.spPrize_Insert", param, commandType: CommandType.StoredProcedure);

        // we then bind it (after being filled) with the actual model
        prize.Id = param.Get<int>("@Id");
    }

    public void CreateTeam(TeamModel team)
    {
        using IDbConnection connection =
            new System.Data.SqlClient.SqlConnection(GlobalConfig.GetConnectionString(DatabaseName));

        var param = new DynamicParameters();
        param.Add("@TeamName", team.TeamName);
        param.Add("@Id", null, dbType: DbType.Int32, direction: ParameterDirection.Output);

        // TODO - Wrap creating a team into a transaction

        connection.Execute("dbo.spTeam_Insert", param, commandType: CommandType.StoredProcedure);

        team.Id = param.Get<int>("@Id");

        foreach (PersonModel p in team.TeamMembers)
        {
            param = new DynamicParameters();
            param.Add("@TeamId", team.Id);
            param.Add("@PersonId", p.Id);

            connection.Execute("dbo.spTeamMember_Insert", param, commandType: CommandType.StoredProcedure);
        }
    }

    public void CreateTournament(TournamentModel tournament)
    {
        using IDbConnection connection =
            new System.Data.SqlClient.SqlConnection(GlobalConfig.GetConnectionString(DatabaseName));

        var param = new DynamicParameters();
        param.Add("@TournamentName", tournament.TournamentName);
        param.Add("@EntryFee", tournament.EntryFee);
        param.Add("@Id", null, dbType: DbType.Int32, direction: ParameterDirection.Output);

        // TODO - Wrap creating a tournament into a transaction

        connection.Execute("dbo.spTournament_Insert", param, commandType: CommandType.StoredProcedure);

        tournament.Id = param.Get<int>("@Id");

        connection.SaveTournamentEntries(tournament);
        connection.SaveTournamentPrizes(tournament);
        connection.SaveTournamentRounds(tournament);
    }

    public List<PersonModel> GetPerson_All()
    {
        var output = new List<PersonModel>();

        using IDbConnection connection =
            new System.Data.SqlClient.SqlConnection(GlobalConfig.GetConnectionString(DatabaseName));

        output = connection.Query<PersonModel>("dbo.spPerson_GetAll"
            , commandType: CommandType.StoredProcedure).ToList();

        return output;
    }

    public List<TeamModel> GetTeam_All()
    {
        List<TeamModel> output;

        using IDbConnection connection =
            new System.Data.SqlClient.SqlConnection(GlobalConfig.GetConnectionString(DatabaseName));
                
        output = connection.Query<TeamModel>("dbo.spTeam_GetAll"
            , commandType: CommandType.StoredProcedure).ToList();

        foreach (var team in output)
        {
            team.TeamMembers = connection.GetPerson_ByTeam(team.Id);
        }
        
        return output;
    }

    public List<TournamentModel> GetTournament_All()
    {
        List<TournamentModel> output;

        using IDbConnection connection =
            new System.Data.SqlClient.SqlConnection(GlobalConfig.GetConnectionString(DatabaseName));
                
        output = connection.Query<TournamentModel>("dbo.spTournament_GetAll").ToList();

        foreach (var tournament in output)
        {
            // Load teams
            tournament.EnteredTeams = connection.GetTeam_ByParameter(
                "dbo.spTournamentEntry_GetByTournament", 
                "@TournamentId", 
                tournament.Id);

            // Load prizes
            tournament.Prizes = connection.GetPrize_ByTournament(tournament.Id);

            // Load rounds
            tournament.Rounds = connection.LoadTournamentRounds(tournament);
        }        

        return output;
    }

    public void UpdateMatchup(MatchupModel matchup)
    {
        using IDbConnection connection =
            new System.Data.SqlClient.SqlConnection(GlobalConfig.GetConnectionString(DatabaseName));

        var param = new DynamicParameters();
        param.Add("@Id", matchup.Id);
        param.Add("@WinnerId", matchup.Winner?.Id);

        connection.Execute("dbo.spMatchup_Update", param, commandType: CommandType.StoredProcedure);

        foreach (MatchupEntryModel entry in matchup.Entries)
        {
            if (entry.TeamCompeting == null) continue;

            param = new DynamicParameters();
            param.Add("@Id", entry.Id);
            // We need the method to blow up on null TeamCompeting
            // because we do not want to update the score for an entry with no team (yet)
            param.Add("@TeamId", entry.TeamCompeting.Id);
            param.Add("@Score", entry.Score);

            connection.Execute("dbo.spMatchupEntry_Update", param, commandType: CommandType.StoredProcedure);
        }
    }
}
