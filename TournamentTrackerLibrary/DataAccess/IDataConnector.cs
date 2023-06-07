using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess
{
    public interface IDataConnector
    {
        void CreatePerson(PersonModel model);
        void CreatePrize(PrizeModel model);
        void CreateTeam(TeamModel model);
        void CreateTournament(TournamentModel model);
        List<PersonModel> GetPerson_All();
        List<TeamModel> GetTeam_All();
        List<TournamentModel> GetTournament_All();
        void UpdateMatchup(MatchupModel model);
    }
}
