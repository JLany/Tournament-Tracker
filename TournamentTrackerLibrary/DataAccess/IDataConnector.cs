using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess
{
    public interface IDataConnector
    {
        void CreatePerson(PersonModel model);
        void CreatePrize(PrizeModel model);
        void CreateTeam(TeamModel model);
        /// <summary>
        /// Setup a <see cref="TournamentModel"/>'s internal logic and data, then store it to data storage medium.
        /// </summary>
        /// <param name="model"></param>
        void CreateTournament(TournamentModel model);
        List<PersonModel> GetPerson_All();
        List<TeamModel> GetTeam_All();
        List<TournamentModel> GetTournament_All();
        /// <summary>
        /// Update a <see cref="MatchupModel"/> in data storage, including updating its Entries.
        /// </summary>
        /// <param name="model"></param>
        void UpdateMatchup(MatchupModel model);
        void CompleteTournament(TournamentModel model);
    }
}
