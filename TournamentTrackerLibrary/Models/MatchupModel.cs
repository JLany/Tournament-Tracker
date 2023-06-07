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

        public string MatchupSummary
        {
            get
            {
                // TODO - Could be better

                string output = "";

                foreach (var entry in Entries)
                {
                    if (output.Length == 0)
                    {
                        if (entry.TeamCompeting != null)
                        {
                            output += entry.TeamCompeting?.TeamName;
                        }
                        else
                        {
                            output += "TBD";
                        }
                    }
                    else
                    {
                        if (entry.TeamCompeting != null)
                        {
                            output += $" vs. {entry.TeamCompeting?.TeamName}";
                        }
                        else
                        {
                            output += " vs. TBD";
                        }
                    }
                }

                return output;
            }
        }
    }
}
