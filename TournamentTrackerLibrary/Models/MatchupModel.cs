namespace TournamentTrackerLibrary.Models
{
    public class MatchupModel : IDataModel
    {
        public int Id { get; set; }
        public List<MatchupEntryModel> Entries { get; set; }
        public TeamModel Winner { get; set; }
        public int MatchupRound { get; set; }
    }
}
