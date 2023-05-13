using Dapper;
using System.Data;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess
{
    public class SqlServerConnector : IDataConnection
    {
        public PrizeModel CreatePrize(PrizeModel model)
        {
            // TODO - Implement the actual sql server storage logic

            using (IDbConnection dbConnection = new System.Data.SqlClient.SqlConnection(GlobalConfig.GetConnectionString("Tournaments")))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@place_number", model.PlaceNumber);
                parameters.Add("@place_name", model.PlaceName);
                parameters.Add("@prize_amount", model.PrizeAmount);
                parameters.Add("@prize_percentage", model.PrizePercentage);
                
                // send @prize_id as a parameter to be filled by sql server; more specifically by the stored procedure
                parameters.Add("@prize_id", null, dbType: DbType.Int32, direction: ParameterDirection.Output);

                // through the execution of the stored procedure, it will fill @prize_id
                dbConnection.Execute("dbo.sp_prize_insert", parameters, commandType: CommandType.StoredProcedure);

                // we then bind it (after being filled) with the actual model
                model.Id = parameters.Get<int>("@prize_id");
            }

            return model;
        }
    }
}
