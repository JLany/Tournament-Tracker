namespace TournamentTrackerLibrary.Models
{
    public class TournamentModel : IDataModel
    {
        public int Id { get; set; }
        public string TournamentName { get; set; }
        public decimal EntryFee { get; set; }
        public List<TeamModel> EnteredTeams { get; set; } = new();
        public List<PrizeModel> Prizes { get; set; } = new();
        public List<Round> Rounds { get; set; } = new();
        /// <summary>
        /// Current-active-round's number with not played matchups yet.
        /// </summary>
        public int CurrentRound
        {
            get
            {
                int output = 1;
                Rounds.ForEach(r => { if (r.Matchups.All(m => m.Winner != null)) ++output; });
                return output;
            }
        }

        public TournamentModel() { }

        /// <summary>
        /// Fail tolerant constructor
        /// </summary>
        /// <param name="tournamentName"></param>
        /// <param name="entryFee"></param>
        /// <param name="teams"></param>
        /// <param name="prizes"></param>
        public TournamentModel(string tournamentName, string entryFee
            , List<TeamModel> teams, List<PrizeModel> prizes) 
        {
            TournamentName = tournamentName;

            decimal.TryParse(entryFee, out decimal entryFeeValue);
            EntryFee = entryFeeValue;

            EnteredTeams = teams;
            Prizes = prizes;
        }
    }
}
