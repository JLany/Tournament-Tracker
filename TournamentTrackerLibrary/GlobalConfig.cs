using System.Configuration;
using TournamentTrackerLibrary.DataAccess;

namespace TournamentTrackerLibrary
{
    public static class GlobalConfig
    {
        public static IDataConnector Connector { get; private set; }

        public static bool InitializeDataConnector(DataConnectionType dataConnction)
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

        public const string PersonFile = "PersonModels.csv";
        public const string PrizeFile = "PrizeModels.csv";
        public const string TeamFile = "TeamModels.csv";
        public const string TournamentFile = "TournamentModels.csv";
        public const string MatchupFile = "MatchupModels.csv";
        public const string MatchupEntryFile = "MatchupEntryModels.csv";
    }
}
