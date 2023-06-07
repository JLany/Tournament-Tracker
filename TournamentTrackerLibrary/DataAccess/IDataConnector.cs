using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess
{
    public interface IDataConnector
    {
        PersonModel CreatePerson(PersonModel model);
        PrizeModel CreatePrize(PrizeModel model);
        TeamModel CreateTeam(TeamModel model);
        TournamentModel CreateTournament(TournamentModel model);
        List<PersonModel> GetPerson_All();
        List<TeamModel> GetTeam_All();
        List<TournamentModel> GetTournament_All();
        MatchupModel UpdateMatchup(MatchupModel model);
    }
}
