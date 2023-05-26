using Dapper;
using System.Data;
using System.Reflection;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess
{
    public class SqlServerConnector : IDataConnector
    {
        public PersonModel CreatePerson(PersonModel model)
        {
            using (IDbConnection connection = 
                new System.Data.SqlClient.SqlConnection(GlobalConfig.GetConnectionString("Tournaments")))
            {
                var param = new DynamicParameters();

                param.Add("@FirstName", model.FirstName);
                param.Add("@LastName", model.LastName);
                param.Add("@EmailAddress", model.EmailAddress);
                param.Add("@PhoneNumber", model.PhoneNumber);
                param.Add("@Id", model.Id, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPerson_Insert", param, commandType: CommandType.StoredProcedure);

                model.Id = param.Get<int>("@Id");
            }

            return model;
        }

        public PrizeModel CreatePrize(PrizeModel model)
        {
            using (IDbConnection connection = 
                new System.Data.SqlClient.SqlConnection(GlobalConfig.GetConnectionString("Tournaments")))
            {
                var param = new DynamicParameters();

                param.Add("@PlaceNumber", model.PlaceNumber);
                param.Add("@PlaceName", model.PlaceName);
                param.Add("@PrizeAmount", model.PrizeAmount);
                param.Add("@PrizePercentage", model.PrizePercentage);

                // send @Id as a parameter to be filled by sql server; more specifically by the stored procedure
                param.Add("@I", null, dbType: DbType.Int32, direction: ParameterDirection.Output);

                // through the execution of the stored procedure, it will fill @Id
                connection.Execute("dbo.sp_prize_insert", param, commandType: CommandType.StoredProcedure);

                // we then bind it (after being filled) with the actual model
                model.Id = param.Get<int>("@Id");
            }

            return model;
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
    }
}
