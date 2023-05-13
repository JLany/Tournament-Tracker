namespace TournamentTrackerLibrary.Models
{
    public class MatchupModel
    {
        public List<MatchupEntryModel> Entries { get; set; }
        public TeamModel Winner { get; set; }
        public int MatchupRound { get; set; }
    }
}
