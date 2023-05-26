using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess.SqlConnectorHelper
{
    public static class DbConnectionHelper
    {
        public static void CreateTeamMember(this IDbConnection connection
            , TeamModel team, PersonModel person)
        {
            var param = new DynamicParameters();
            param.Add("@TeamId", team.Id);
            param.Add("@PersonId", person.Id);

            connection.Execute("dbo.spTeamMember_Insert", param, commandType: CommandType.StoredProcedure);
        }
    }
}
