namespace TournamentTrackerLibrary.Models
{
    public class MatchupEntryModel
    {
        /// <summary>
        /// Team particepating in the matchup
        /// </summary>
        public TeamModel TeamCompeting { get; set; }
        public double Score { get; set; }
        public MatchupModel ParentMatchup { get; set; }
    }
}
