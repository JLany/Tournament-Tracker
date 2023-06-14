﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary;

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

    public static void LogWinner(MatchupModel matchup)
    {
        // It is now guarnteed that there are two entries in matchup, thanks to PlayByeWeeks.

        // TODO - (OPTIONAL) Try to do better
        //MatchupEntryModel teamOneEntry = matchup.Entries.First();
        //MatchupEntryModel teamTwoEntry = matchup.Entries.ElementAtOrDefault(1);

        if (matchup.Entries.Count == 1)
        {
            matchup.Winner = matchup.Entries.First().TeamCompeting;
        }
        else
        {
            matchup.Winner = DetermineWinner(matchup);
        }
        //else if (teamOneEntry.Score > teamTwoEntry.Score)
        //{
        //    matchup.Winner = teamOneEntry.TeamCompeting;
        //}
        //else if (teamTwoEntry.Score > teamOneEntry.Score)
        //{
        //    matchup.Winner = teamTwoEntry.TeamCompeting;
        //}
        //else
        //{
        //    throw new InvalidOperationException();
        //}


        GlobalConfig.Connector.UpdateMatchup(matchup);
    }

    public static int NumberOfRounds(int teamCount)
    {
        return (int)Math.Ceiling(Math.Log2(teamCount));
    }

    public static void PlayByeWeeks(this TournamentModel tournament)
    {
        Round roundOne = tournament.Rounds.First();

        // Find bye week matchups, and mark team competing at them as the winner.
        foreach (var matchup in roundOne.Matchups.Where(m => m.Entries.Count == 1))
        {
            LogWinner(matchup);
            QualifyWinnerToNextRound(tournament, matchup);
            GlobalConfig.Connector.UpdateMatchup(matchup);
        }
    }

    /// <summary>
    /// Setup a <see cref="TournamentModel"/>'s initial state and store it to data storage.
    /// <br></br>
    /// Model must not be used before calling this method.
    /// </summary>
    /// <param name="tournament"></param>
    public static void SetupTournament(TournamentModel tournament)
    {
        tournament.CreateRounds();

        GlobalConfig.Connector.CreateTournament(tournament);

        tournament.PlayByeWeeks();

        tournament.RoundBeginningNotification();
    }

    public static void UpdateMatchupResult(TournamentModel tournament, MatchupModel matchup
        , double teamOneScore, double teamTwoScore)
    {
        matchup.Entries.ElementAt(0).Score = teamOneScore;
        matchup.Entries.ElementAt(1).Score = teamTwoScore;

        int currentRound = tournament.CurrentRound;

        LogWinner(matchup);
        QualifyWinnerToNextRound(tournament, matchup);

        if (currentRound < tournament.CurrentRound)
        {
            tournament.RoundBeginningNotification();
        }
    }

    private static void RoundBeginningNotification(this TournamentModel tournament)
    {
        if (tournament.CurrentRound > tournament.Rounds.Count)
        {
            // Tournament has finished
            return;
        }

        Round currentRound = tournament.Rounds
            .Where(r => r.Matchups.First().MatchupRound == tournament.CurrentRound)
            .First();

        foreach (var matchup in currentRound.Matchups)
        {
            if (matchup.Entries.Count == 1)
            {
                continue;
            }
                
            foreach (var entry in matchup.Entries)
            {
                foreach (var person in entry.TeamCompeting?.TeamMembers ?? new List<PersonModel>())
                {
                    RoundBeginningNotification(person, entry.TeamCompeting.TeamName
                        , matchup.Entries.Where(e => e.Id != entry.Id).First());
                }
            }
        }
    }

    private static void RoundBeginningNotification(PersonModel person, string teamName
        , MatchupEntryModel competitor)
    {
        var sender = GlobalConfig.GetSenderEmailAddress();
        var to = new List<string> { person.EmailAddress };
        var subject = $"{teamName}: You have a new matchup against {competitor.TeamCompeting.TeamName}";
        var body = new StringBuilder();

        body.AppendLine("<h1>You have a new matchup</h1>");
        body.Append("<p><strong>Competitor: </strong>");
        body.AppendLine($"{competitor.TeamCompeting.TeamName}</p>");
        body.AppendLine();
        body.AppendLine("<br><br><p>Have a great time!</p>");
        body.AppendLine("<br>~Tournament Tracker");

        EmailLogic.SendEmail(to, subject, body.ToString());
    }

    private static TeamModel DetermineWinner(MatchupModel matchup)
    {
        return GlobalConfig.ScoreComparator(
            matchup.Entries.ElementAt(0),
            matchup.Entries.ElementAt(1)
            )
            .TeamCompeting;
    }

    private static void QualifyWinnerToNextRound(TournamentModel tournament, MatchupModel matchup)
    {
        foreach (var round in tournament.Rounds)
        {
            foreach (var childMatchup in round.Matchups)
            {
                foreach (var entry in childMatchup.Entries)
                {
                    if (entry.ParentMatchup?.Id == matchup.Id)
                    {
                        entry.TeamCompeting = matchup.Winner;

                        GlobalConfig.Connector.UpdateMatchup(childMatchup);

                        return;
                    }
                }
            }
        }
    }

    private static Round CreateFirstRound(List<TeamModel> teams, int nByes)
    {
        var matchups = new List<MatchupModel>();
        var matchup = new MatchupModel();

        foreach (var team in teams)
        {
            matchup.Entries.Add(new MatchupEntryModel { TeamCompeting = team });

            // If: there are still unused byes, or we have already two entries.
            if (nByes > 0 || matchup.Entries.Count > 1)
            {
                // First round.
                matchup.MatchupRound = 1; 
                matchups.Add(matchup);

                matchup = new MatchupModel();

                if (nByes > 0)
                {
                    --nByes;
                }
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
                    // round is zero based, so we add 1.
                    matchup.MatchupRound = round + 1; 

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

    private static List<TeamModel> RandomizeTeams(List<TeamModel> teams)
    {
        return teams.OrderBy(t => Guid.NewGuid()).ToList();
    }
}
