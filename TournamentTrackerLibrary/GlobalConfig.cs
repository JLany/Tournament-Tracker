using System.Configuration;
using TournamentTrackerLibrary.DataAccess;

namespace TournamentTrackerLibrary
{
    public static class GlobalConfig
    {
        public static IDataConnector Connector { get; private set; }

        public static bool InitializeConnection(DataConnectionType dataConnction)
        {
            switch (dataConnction)
            {
                case DataConnectionType.SqlServer:
                    var sqlServerCnn = new SqlServerConnector();
                    Connector = sqlServerCnn;
                    break;
                case DataConnectionType.TextFile:
                    var textFileCnn = new TextFileConnector();
                    Connector = textFileCnn;
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
