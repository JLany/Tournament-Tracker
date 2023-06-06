using TournamentTrackerLibrary.DataAccess;

namespace TournamentTrackerUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            TournamentTrackerLibrary.GlobalConfig.InitializeConnection(DataConnectionType.SqlServer);

            Application.Run(new TournamentDashboardForm());
            // Application.Run(new TournamentDashboardForm());
        }
    }
}