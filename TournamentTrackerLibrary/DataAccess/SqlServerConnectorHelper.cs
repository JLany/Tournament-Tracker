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

    public static void SaveTournamentRounds(this IDbConnection connection
        , TournamentModel tournament)
    {
        // foreach round
        //     foreach matchup
        //         save matchup
        //         save matchup entries
        //     endfor
        // endfor


        foreach (List<MatchupModel> round in tournament.Rounds)
        {
            foreach (var matchup in round)
            {
                var param = new DynamicParameters();
                param.Add("@TournamentId", tournament.Id);
                param.Add("@MatchupRound", matchup.MatchupRound);
                param.Add("@Id", null, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spMatchup_Insert", param, commandType: CommandType.StoredProcedure);

                matchup.Id = param.Get<int>("@Id");

                foreach (MatchupEntryModel entry in matchup.Entries)
                {
                    param = new DynamicParameters();
                    param.Add("@MatchupId", matchup.Id);
                    param.Add("@TeamId", entry.TeamCompeting?.Id);
                    param.Add("@ParentMatchupId", entry.ParentMatchup?.Id);
                    param.Add("@Id", null, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("dbo.spMatchupEntry_Insert"
                        , param, commandType: CommandType.StoredProcedure);

                    entry.Id = param.Get<int>("@Id");
                }
            }
        }
    }
}
