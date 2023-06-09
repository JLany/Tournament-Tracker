﻿namespace TournamentTrackerLibrary.Models
{
    /// <summary>
    /// Represens one team in a matchup
    /// </summary>
    public class MatchupEntryModel : IDataModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Team particepating in the matchup
        /// </summary>
        public TeamModel? TeamCompeting { get; set; }
        public double Score { get; set; }

        /// <summary>
        /// Unique database identifier for ParentMatchup.<br></br> Used to acquire ParentMatchup.
        /// </summary>
        public int ParentMatchupId { get; set; }
        public MatchupModel? ParentMatchup { get; set; }
    }
}
