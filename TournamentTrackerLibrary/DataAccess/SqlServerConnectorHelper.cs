using Dapper;
using System.Data;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess.SqlServerConnectorHelper;

public static class SqlServerConnectorHelper
{
    public static void CreateTeamMember(this IDbConnection connection
        , TeamModel team, PersonModel person)
    {
        var param = new DynamicParameters();
        param.Add("@TeamId", team.Id);
        param.Add("@PersonId", person.Id);

        connection.Execute("dbo.spTeamMember_Insert", param, commandType: CommandType.StoredProcedure);
    }

    public static void SaveTournamentEntries(this IDbConnection connection
        , TournamentModel tournament)
    {
        foreach (var team in tournament.EnteredTeams)
        {
            var param = new DynamicParameters();
            param.Add("@TournamentId", tournament.Id);
            param.Add("@TeamId", team.Id);

            connection.Execute("dbo.spTournamentEntry_Insert", param, commandType: CommandType.StoredProcedure);
        }
    }

    public static void SaveTournamentPrizes(this IDbConnection connection
        , TournamentModel tournament)
    {
        foreach (var prize in tournament.Prizes)
        {
            var param = new DynamicParameters();
            param.Add("@TournamentId", tournament.Id);
            param.Add("@PrizeId", prize.Id);

            connection.Execute("dbo.spTournamentPrize_Insert", param, commandType: CommandType.StoredProcedure);
        }
    }
}
