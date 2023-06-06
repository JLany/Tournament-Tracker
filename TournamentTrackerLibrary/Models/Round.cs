using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentTrackerLibrary.Models
{
    public class Round
    {
        public List<MatchupModel> Matchups { get; set; } = new();

        public override string ToString()
        {
            int round = Matchups.First().MatchupRound;
            return $"Round {round}";
        }
    }
}
