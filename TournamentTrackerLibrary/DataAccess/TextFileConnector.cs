using TournamentTrackerLibrary.DataAccess.TextFileHelpers;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess
{
    public class TextFileConnector : IDataConnector
    {
        // TODO - (OPTIONAL) Take a look at AutoMapper, useful to learn

        private const string PersonsFile = "PersonModels.csv";
        private const string PrizesFile = "PrizeModel.csv";
        private const string TeamsFile = "TeamModels.csv";

        public PersonModel CreatePerson(PersonModel person)
        {
            // Load text file contents and convert them to List<PrizeModel>
            var persons = GetPerson_All();

            // Find the max Id 
            int currentId = 1;

            if (persons.Count > 0)
            {
                currentId = persons.OrderByDescending(p => p.Id).First().Id + 1;
            }

            // Assign the new Id to model
            person.Id = currentId;

            // Add model to prizes list
            persons.Add(person);

            // Convert prizes list to List<string> and save text to the text file
            persons.SaveToModelFile(PersonsFile);

            return person;
        }

        public PrizeModel CreatePrize(PrizeModel prize)
        {
            // Load text file contents and convert them to List<PrizeModel>
            var prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            // Find the max Id 
            int currentId = 1;

            if (prizes.Count > 0)
            {
                currentId = prizes.OrderByDescending(p => p.Id).First().Id + 1;
            }

            // Assign the new Id to model
            prize.Id = currentId;

            // Add model to prizes list
            prizes.Add(prize);

            // Convert prizes list to List<string> and save text to the text file
            prizes.SaveToModelFile(PrizesFile);

            return prize;
        }

        public TeamModel CreateTeam(TeamModel team)
        {
            // Load all teams from teams text file
            var teams = TeamsFile.FullFilePath().LoadFile().ConvertToTeamModels(PersonsFile);

            // Find the max Id
            int currentId = 1;

            if (teams.Count > 0)
            {
                currentId = teams.OrderByDescending(t => t.Id).First().Id + 1;
            }

            // Assign the new Id to our model
            team.Id = currentId;

            // Add model to teams list
            teams.Add(team);

            // Convert to list of string, and save to file
            teams.SaveToTeamModelFile(TeamsFile);

            return team;
        }

        public List<PersonModel> GetPerson_All()
        {
            return PersonsFile.FullFilePath().LoadFile().ConvertToPersonModels();
        }

        public List<TeamModel> GetTeam_All()
        {
            return TeamsFile.FullFilePath().LoadFile().ConvertToTeamModels(PersonsFile);
        }
    }
}
