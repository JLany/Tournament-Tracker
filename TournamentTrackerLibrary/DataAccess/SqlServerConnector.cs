using Dapper;
using System.Data;
using System.Reflection;
using TournamentTrackerLibrary.DataAccess.SqlServerConnectorHelper;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess
{
    public class SqlServerConnector : IDataConnector
    {
        public PersonModel CreatePerson(PersonModel person)
        {
            using (IDbConnection connection = 
                new System.Data.SqlClient.SqlConnection(GlobalConfig.GetConnectionString("Tournaments")))
            {
                var param = new DynamicParameters();
                param.Add("@FirstName", person.FirstName);
                param.Add("@LastName", person.LastName);
                param.Add("@EmailAddress", person.EmailAddress);
                param.Add("@PhoneNumber", person.PhoneNumber);
                param.Add("@Id", person.Id, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPerson_Insert", param, commandType: CommandType.StoredProcedure);

                person.Id = param.Get<int>("@Id");
            }

            return person;
        }

        public PrizeModel CreatePrize(PrizeModel prize)
        {
            using (IDbConnection connection = 
                new System.Data.SqlClient.SqlConnection(GlobalConfig.GetConnectionString("Tournaments")))
            {
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

            return prize;
        }

        public TeamModel CreateTeam(TeamModel team)
        {
            using (IDbConnection connection =
                new System.Data.SqlClient.SqlConnection(GlobalConfig.GetConnectionString("Tournaments")))
            {
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

            return team;
        }

        public TournamentModel CreateTournament(TournamentModel tournament)
        {
            using (IDbConnection connection =
                new System.Data.SqlClient.SqlConnection(GlobalConfig.GetConnectionString("Tournaments")))
            {
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

            return tournament;
        }

        public List<PersonModel> GetPerson_All()
        {
            var output = new List<PersonModel>();

            using (IDbConnection connection =
                new System.Data.SqlClient.SqlConnection(GlobalConfig.GetConnectionString("Tournaments")))
            {
                output = connection.Query<PersonModel>("dbo.spPerson_GetAll"
                    , commandType: CommandType.StoredProcedure).ToList();
            }

            return output;
        }

        public List<TeamModel> GetTeam_All()
        {
            var output = new List<TeamModel>();

            using (IDbConnection connection =
                new System.Data.SqlClient.SqlConnection(GlobalConfig.GetConnectionString("Tournaments")))
            {
                output = connection.Query<TeamModel>("dbo.spTeam_GetAll"
                    , commandType: CommandType.StoredProcedure).ToList();

                foreach (var team in output)
                {
                    var param = new DynamicParameters();
                    param.Add("@TeamId", team.Id);

                    team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMember_GetByTeam"
                        , param, commandType: CommandType.StoredProcedure).ToList();
                }
            }

            return output;
        }

        public List<TournamentModel> GetTournament_All()
        {
            throw new NotImplementedException();
        }
    }
}
