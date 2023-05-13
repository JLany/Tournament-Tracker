using System.Configuration;
using System.Runtime.CompilerServices;
using TournamentTrackerLibrary.DataAccess;

namespace TournamentTrackerLibrary
{
    public static class GlobalConfig
    {
        public static IDataConnection Connection { get; private set; }

        public static bool InitializeConnection(DataConnectionType dataConnction)
        {
            switch (dataConnction)
            {
                case DataConnectionType.SqlServer:
                    var sqlServerCnn = new SqlServerConnector();
                    Connection = sqlServerCnn;
                    break;
                case DataConnectionType.TextFile:
                    var textFileCnn = new TextFileConnector();
                    Connection = textFileCnn;
                    break;
                default:
                    return false;
            }

            return true;
        }

        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
