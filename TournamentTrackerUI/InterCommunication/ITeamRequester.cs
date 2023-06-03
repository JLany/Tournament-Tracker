using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerUI.InterCommunication
{
    public interface ITeamRequester
    {
        void ReceiveTeam(TeamModel team);
    }
}
