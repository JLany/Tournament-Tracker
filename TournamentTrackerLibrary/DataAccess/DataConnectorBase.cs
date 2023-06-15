using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess;

public abstract class DataConnectorBase : IDataConnector
{
    public void CreateTournament(TournamentModel tournament)
    {
        TournamentLogic.SetupTournament(tournament);
        CreateTournamentImpl(tournament);
        TournamentLogic.StartUpTournament(tournament);
    }

    public abstract void CompleteTournament(TournamentModel model);
    public abstract void CreatePerson(PersonModel model);
    public abstract void CreatePrize(PrizeModel model);
    public abstract void CreateTeam(TeamModel model);
    public abstract List<PersonModel> GetPerson_All();
    public abstract List<TeamModel> GetTeam_All();
    public abstract List<TournamentModel> GetTournament_All();
    public abstract void UpdateMatchup(MatchupModel model);
    protected abstract void CreateTournamentImpl(TournamentModel tournament);
}
