using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary
{
    public static class TournamentLogic
    {
        public static void CreateRounds(this TournamentModel tournament)
        {
            // Order list of teams randomly
            var randomizedTeams = RandomizeTeams(tournament.EnteredTeams);

            // Check if the teams count is a power of 2
            int nRounds = NumberOfRounds(randomizedTeams.Count);
            int nByes = NumberOfByes(nRounds, randomizedTeams.Count);

            // Create first round of matchups (e.g. 16 matchups)
            // Add byes if necessary
            tournament.Rounds.Add(CreateFirstRound(randomizedTeams, nByes));

            // Create latter rounds (8 matchups, 4 matchups, 2 matchups, 1 matchup)
            CreateOtherRounds(tournament, nRounds);
        }

        private static Round CreateFirstRound(List<TeamModel> teams, int nByes)
        {
            var matchups = new List<MatchupModel>();
            var matchup = new MatchupModel();

            foreach (var team in teams)
            {
                matchup.Entries.Add(new MatchupEntryModel { TeamCompeting = team });

                // If: there are still unused byes, or we have already two entries 
                if (nByes > 0 || matchup.Entries.Count > 1)
                {
                    matchup.MatchupRound = 1; // first round
                    matchups.Add(matchup);
                    matchup = new MatchupModel();

                    if (nByes > 0) 
                        --nByes;
                }
            }

            return new Round { Matchups = matchups };
        }

        private static void CreateOtherRounds(TournamentModel tournament,  int nRounds)
        {
            // Asuming that first round is created and initialized
            for (int round = 1; round < nRounds; ++round)
            {
                var previousRound = tournament.Rounds[round - 1];
                var thisRound = new Round();
                var matchup = new MatchupModel();

                foreach (var parentMatchup in previousRound.Matchups)
                {
                    matchup.Entries.Add(new MatchupEntryModel { ParentMatchup = parentMatchup });

                    if (matchup.Entries.Count > 1)
                    {
                        matchup.MatchupRound = round + 1; // round is zero based, so we add 1

                        thisRound.Matchups.Add(matchup);

                        matchup = new MatchupModel();
                    }
                }

                tournament.Rounds.Add(thisRound);
            }
        }

        private static int NumberOfByes(int nRounds, int nTeams)
        {
            return (int)Math.Pow(2, nRounds) - nTeams;
        }

        public static int NumberOfRounds(int teamCount)
        {
            return (int)Math.Ceiling(Math.Log2(teamCount));
        }

        private static List<TeamModel> RandomizeTeams(List<TeamModel> teams)
        {
            return teams.OrderBy(t => Guid.NewGuid()).ToList();
        }
    }
}
