using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess.SqlServerConnectorHelper
{
    public static class SqlServerConnectorHelper
    {
        public static void CreateTeamMember(this SqlServerConnector connector
            , TeamModel team, PersonModel person, IDbConnection connection)
        {
            var param = new DynamicParameters();
            param.Add("@TeamId", team.Id);
            param.Add("@PersonId", person.Id);

            connection.Execute("dbo.spTeamMember_Insert", param, commandType: CommandType.StoredProcedure);
        }
    }
}
