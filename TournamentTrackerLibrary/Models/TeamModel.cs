namespace TournamentTrackerLibrary.Models
{
    public class TeamModel : IDataModel
    {
        public int Id { get; set; }
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();
        public string TeamName;
    }
}
