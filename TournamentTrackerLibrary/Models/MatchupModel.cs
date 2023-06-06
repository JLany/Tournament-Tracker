namespace TournamentTrackerLibrary.Models
{
    public class MatchupModel : IDataModel
    {
        public int Id { get; set; }
        public List<MatchupEntryModel> Entries { get; set; } = new();

        /// <summary>
        /// Unique database identifier for Winner.<br></br> Used to retrieve Winner from database.
        /// </summary>
        public int WinnerId { get; set; }
        public TeamModel? Winner { get; set; }
        public int MatchupRound { get; set; }
    }
}
