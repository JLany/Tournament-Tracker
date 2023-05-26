using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess
{
    public interface IDataConnector
    {
        PersonModel CreatePerson(PersonModel model);
        PrizeModel CreatePrize(PrizeModel model);
        TeamModel CreateTeam(TeamModel model);
        List<PersonModel> GetPerson_All();
    }
}
