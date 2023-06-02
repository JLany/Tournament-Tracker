namespace TournamentTrackerLibrary.Models
{
    public class MatchupModel : IDataModel
    {
        public int Id { get; set; }
        public List<MatchupEntryModel> Entries { get; set; } = new();
        public TeamModel Winner { get; set; } = new();
        public int MatchupRound { get; set; }
    }
}
