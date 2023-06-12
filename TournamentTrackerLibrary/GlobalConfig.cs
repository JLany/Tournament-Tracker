using System.Configuration;
using TournamentTrackerLibrary.DataAccess;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary
{
    public static class GlobalConfig
    {
        public const string PersonFile = "PersonModels.csv";
        public const string PrizeFile = "PrizeModels.csv";
        public const string TeamFile = "TeamModels.csv";
        public const string TournamentFile = "TournamentModels.csv";
        public const string MatchupFile = "MatchupModels.csv";
        public const string MatchupEntryFile = "MatchupEntryModels.csv";
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

        // This logic does not make any sense.
        // The predicate should depend on the tournament and be stored with the
        // tournament's data, rather than an app setting.
        // Should be changed in future versions, when adding support to choose 
        // the winning predicate.
        public static ScoreComparator ScoreComparator
        {
            get
            {
                string? predicate = ConfigurationManager.AppSettings["greaterScoreWins"];

                return predicate switch
                {
                    "true" => GreaterWins,
                    "false" => LesserWins,
                    _ => throw new ConfigurationErrorsException("Score predicate in AppSettings is invalid."),
                };
            }
        }

        public static string GetTextFilesDir() 
            => ConfigurationManager.AppSettings["textFilesDirectory"];        

        private static MatchupEntryModel GreaterWins(MatchupEntryModel a, MatchupEntryModel b)
        {
            if (a.Score > b.Score)
            {
                return a;
            }
            else if (b.Score > a.Score)
            {
                return b;
            }
            else
            {
                throw new InvalidOperationException("Ties are not allowed in this context");
            }
        }

        private static MatchupEntryModel LesserWins(MatchupEntryModel a, MatchupEntryModel b)
        {
            if (a.Score < b.Score)
            {
                return a;
            }
            else if (b.Score < a.Score)
            {
                return b;
            }
            else
            {
                throw new InvalidOperationException("Ties are not allowed in this context");
            }
        }
    }

    public delegate MatchupEntryModel ScoreComparator(MatchupEntryModel a, MatchupEntryModel b);
}
