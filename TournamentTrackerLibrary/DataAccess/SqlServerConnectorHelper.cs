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

    public static List<PrizeModel> GetPrize_ByTournament(this IDbConnection connection, int tournamentId)
    {
        var param = new DynamicParameters();
        param.Add("@TournamentId", tournamentId);

        List<PrizeModel> output = connection.Query<PrizeModel>("dbo.spTournamentPrize_GetByTournament"
            , param, commandType: CommandType.StoredProcedure).ToList();

        return output;
    }

    public static List<PersonModel> GetPerson_ByTeam(this  IDbConnection connection, int teamId)
    {
        var param = new DynamicParameters();
        param.Add("@TeamId", teamId);

        List<PersonModel> output = connection.Query<PersonModel>("dbo.spTeamMember_GetByTeam"
            , param, commandType: CommandType.StoredProcedure).ToList();

        return output;
    }

    public static List<TeamModel> GetTeam_ByParameter(this IDbConnection connection
        ,string procedureName, string paramName, int paramValue)
    {
        var param = new DynamicParameters();
        param.Add(paramName, paramValue);

        List<TeamModel> output = connection.Query<TeamModel>(procedureName
            , param, commandType: CommandType.StoredProcedure).ToList();

        foreach (var team in output)
        {
            team.TeamMembers = connection.GetPerson_ByTeam(team.Id);
        }

        return output;
    }

    public static List<List<MatchupModel>> LoadTournamentRounds(this  IDbConnection connection
        , TournamentModel tournament)
    {
        int roundsCount = TournamentLogic.NumberOfRounds(tournament.EnteredTeams.Count);
        var rounds = new List<List<MatchupModel>>();
        for (int i = 1; i <= roundsCount; ++i)
            rounds.Add(new List<MatchupModel>());

        var param = new DynamicParameters();
        param.Add("@TournamentId", tournament.Id);

        List<MatchupModel> matchups = connection.Query<MatchupModel>("dbo.spMatchup_GetByTournament"
            , param, commandType: CommandType.StoredProcedure).ToList();


        foreach (var matchup in matchups)
        {
            // Load Entries
            param = new DynamicParameters();
            param.Add("@MatchupId", matchup.Id);

            matchup.Entries = connection.Query<MatchupEntryModel>("dbo.spMatchupEntry_GetByMatchup"
                , param, commandType: CommandType.StoredProcedure).ToList();

            foreach (var entry in matchup.Entries)
            {
                // Load team. We do not always know who is competing yet, so can be null
                entry.TeamCompeting = connection.GetTeam_ByParameter(
                    "dbo.spTeam_GetByMatchupEntry",
                    "@MatchupEntryId",
                    entry.Id)
                    .FirstOrDefault();

                // Load parent matchup
                entry.ParentMatchup = 
                    matchups
                    .Where(m => m.Id == entry.ParentMatchupId)
                    .FirstOrDefault();
            }

            // Load Winner. We do not always have winner yet, so can be null
            matchup.Winner = connection.GetTeam_ByParameter(
                "dbo.spTeam_GetByMatchup", "@MatchupId", matchup.Id)
                .FirstOrDefault();

            // Add to the corresponding round
            rounds[matchup.MatchupRound - 1].Add(matchup); // (zero based)
        }

        return rounds;
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
