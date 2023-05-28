namespace TournamentTrackerLibrary.Models
{
    public class PersonModel : IDataModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        public string FullName
        {
            get => $"{FirstName} {LastName}";
        }
    }
}
