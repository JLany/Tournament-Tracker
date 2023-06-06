﻿using System.Configuration;
using System.Text.RegularExpressions;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess.TextFileHelpers;

public static class TextFileConnectorProcessor
{
    /// <summary>
    /// Connects the storage directory from <see cref="AppSettings"/> 
    /// to the specified file name
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string FullFilePath(this string fileName)
        => $@"{ConfigurationManager.AppSettings["textFilesDirectory"]}\{fileName}";

    /// <summary>
    /// Loads file contents into a <see cref="List{string}"/> of <see cref="string"/>.
    /// Invoked on a full file path
    /// </summary>
    /// <param name="filePath">Absolute full file path</param>
    /// <returns><see cref="List{T}"/> of <see cref="string"/>: lines in the file</returns>
    public static List<string> LoadFile(this string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<string>();
        }

        return File.ReadAllLines(filePath).ToList();
    }

    private static List<MatchupEntryModel> ConvertToMatchupEntryModels(this List<string> lines)
    {
        var matchupEntries = new List<MatchupEntryModel>();
        List<string> parentIds = new();

        foreach (var line in lines)
        {
            //{id,TeamCompeting,Score,ParentMatchup}
            string[] cols = line.Split(',');


            var matchupEntry = new MatchupEntryModel
            {
                Id = int.Parse(cols[0]),
                TeamCompeting = LookupTeamById(cols[1]),
                Score = double.Parse(cols[2]),
                ParentMatchup = LookupMatchupById(cols[3]),
            };

            matchupEntries.Add(matchupEntry);
        }

        return matchupEntries;
    }

    private static List<MatchupModel> ConvertToMatchupModels(this List<string> lines)
    {
        var matchups = new List<MatchupModel>();

        foreach (var line in lines)
        {
            //{id,list|of|entries|ids,WinnerId,MatchupRound}
            string[] cols = line.Split(',');

            var matchup = new MatchupModel
            {
                Id = int.Parse(cols[0]),
                Entries = LookupMatchupEntriesByIds(cols[1]),
                Winner = LookupTeamById(cols[2]),
                MatchupRound = int.Parse(cols[3]),
            };

            matchups.Add(matchup);
        }

        return matchups;
    }

    public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
    {
        var persons = new List<PersonModel>();

        foreach (var line in lines)
        {
             string[] cols = line.Split(',');

            var p = new PersonModel
            {
                Id = int.Parse(cols[0]),
                FirstName = cols[1],
                LastName = cols[2],
                EmailAddress = cols[3],
                PhoneNumber = cols[4]
            };

            persons.Add(p);
        }

        return persons;
    }

    public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
    {
        var prizes = new List<PrizeModel>();

        foreach (var line in lines)
        {
            string[] cols = line.Split(',');

            // Here we do not use the `fail tolerant constructor` 
            // because we do not want to have a half ass built model
            // Alternatively, we want this method to crash when something happens
            // this is due to the fact that we read from a file that is generated by
            // the app, so we do expect the data to be right, the opposite is an Exception!
            // Leaving the app in an uncertain state is not the best thing to do with our own hands
            var p = new PrizeModel
            {
                Id = int.Parse(cols[0]),
                PlaceNumber = int.Parse(cols[1]),
                PlaceName = cols[2],
                PrizeAmount = decimal.Parse(cols[3]),
                PrizePercentage = int.Parse(cols[4])
            };

            prizes.Add(p);
        }

        return prizes;
    }

    public static List<TeamModel> ConvertToTeamModels(this List<string> lines, string personFileName)
    {
        // {id,name,list of members' ids}
        // 17,Eagels,14|5|17|18

        var teams = new List<TeamModel>();
        var persons = personFileName.FullFilePath().LoadFile().ConvertToPersonModels();

        foreach (var line in lines)
        {
            string[] cols = line.Split(',');

            var team = new TeamModel
            {
                Id = int.Parse(cols[0]),
                TeamName = cols[1],
            };

            string[] memberIds = cols[2].Split('|');

            foreach (var memberId in memberIds)
            {
                // get from file of person models
                PersonModel member = persons.Where(p => p.Id == int.Parse(memberId)).First();
                team.TeamMembers.Add(member);
            }

            teams.Add(team);
        }

        return teams;
    }

    public static List<TournamentModel> ConvertToTournamentModels(this List<string> lines)
    {
        var tournaments = new List<TournamentModel>();
        var allMatchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchupModels();
        var allTeams = GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(GlobalConfig.PersonFile);
        var allPrizes = GlobalConfig.PrizeFile.FullFilePath().LoadFile().ConvertToPrizeModels();

        foreach (var line in lines)
        {
            // {id,name,entryFee,active,list of teams' ids,list of prizes' ids,list of rounds' ids}
            // {15,CHAMPS,120,1,5|65|41|17|6,7|3|55|23,6^3^12|8^2|8^1
            string[] cols = line.Split(',');

            var tournament = new TournamentModel
            {
                Id = int.Parse(cols[0]),
                TournamentName = cols[1],
                EntryFee = decimal.Parse(cols[2]),
            };

            string[] enteredTeamsIds = cols[4].Split('|');
            foreach (string id in  enteredTeamsIds)
            {
                tournament.EnteredTeams.Add(allTeams.Where(t => t.Id == int.Parse(id)).First());
            }

            if (cols[5].Length > 0) // if there are prizes
            {
                string[] tournamentPrizesIds = cols[5].Split('|');
                foreach (string id in tournamentPrizesIds)
                {
                    tournament.Prizes.Add(allPrizes.Where(t => t.Id == int.Parse(id)).First());
                }
            }

            string[] rounds = cols[6].Split('|');
            foreach (var round in rounds)
            {
                var matchups = new List<MatchupModel>();
                string[] matchupIds = round.Split('^');
                foreach (var id in  matchupIds)
                {
                    matchups.Add(allMatchups.Where(m => m.Id == int.Parse(id)).First());
                }

                tournament.Rounds.Add(new Round { Matchups = matchups });
            }

            tournaments.Add(tournament);
        }

        return tournaments;
    }

    private static MatchupModel? LookupMatchupById(string id)
    {
        // Here we prevent it from blowing up
        // Because MatchupEntryModels can contain ParentMatchup as null, 
        // thus no id for it in the MatchupEntryFile 
        if (id.Length > 0) 
        {
            var matchups =
                GlobalConfig.MatchupFile
                .FullFilePath()
                .LoadFile();

            foreach (string matchup in matchups)
            {
                string matchupId = matchup.Split(',').First();
                if (matchupId == id)
                {
                    return 
                        new List<string> { matchup }
                        .ConvertToMatchupModels()
                        .First();
                }
            }

            throw new FormatException("Invalid Matchup Id was encountered in MatchupModels.csv");
        }
        else // if empty, then no matchup (yet)
        {
            return null;
        }
    }

    private static List<MatchupEntryModel> LookupMatchupEntriesByIds(string ids)
    {
        var matchupEtnries =
            GlobalConfig.MatchupEntryFile
            .FullFilePath()
            .LoadFile();

        List<string> selectedMatchupEntries = new();

        string[] entriesIds = ids.Split('|');
        foreach (string id in entriesIds)
        {
            foreach (string entry in matchupEtnries)
            {
                string entryId = entry.Split('|').First();
                if (entryId == id)
                {
                    selectedMatchupEntries.Add(entry);
                }
            }
        }

        return selectedMatchupEntries.ConvertToMatchupEntryModels();
    }

    /// <summary>
    /// Lookup for a <em>possibly</em> existing team. <br></br> 
    /// Should not be used when there <em>must</em> be a team!
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private static TeamModel? LookupTeamById(string id)
    {
        // Here we prevent it from blowing up
        // Because TeamCompeteing, Winner can be set as null 
        if (id.Length > 0)
        {
            var teams =
            GlobalConfig.TeamFile
            .FullFilePath()
            .LoadFile()
            .ConvertToTeamModels(GlobalConfig.PersonFile);

            // This way it will blow up on any string other than { "", "<number>" }
            return teams.Where(t => t.Id == int.Parse(id)).First();
        }
        else // if empty, then no team (yet)
        {
            return null;
        }
    }

    private static void CreateMatchupEntries(this List<MatchupEntryModel> matchupEntries)
    {
        var allMatchupEntries =
            GlobalConfig.MatchupEntryFile
            .FullFilePath()
            .LoadFile()
            .ConvertToMatchupEntryModels();

        int currentId = 1;

        if (allMatchupEntries.Count > 0)
        {
            currentId = allMatchupEntries.OrderByDescending(e => e.Id).First().Id + 1;
        }

        foreach (MatchupEntryModel entry in matchupEntries)
        {
            entry.Id = currentId++;
            allMatchupEntries.Add(entry);
        }

        allMatchupEntries.SaveToMatchupEntryFile();
    }

    private static void CreateMatchups(this Round round)
    {
        var allMatchups = 
            GlobalConfig.MatchupFile
            .FullFilePath()
            .LoadFile()
            .ConvertToMatchupModels();

        int currentId = 1;

        if (allMatchups.Count > 0)
        {
            currentId = allMatchups.OrderByDescending(t => t.Id).First().Id + 1;
        }

        foreach (var matchup in round.Matchups)
        {
            matchup.Id = currentId++;
            allMatchups.Add(matchup);

            matchup.Entries.CreateMatchupEntries();
        }

        allMatchups.SaveToMatchupFile();
    }

    public static void SaveRounds(this TournamentModel tournament)
    {
        foreach (var round in tournament.Rounds)
        {
            round.CreateMatchups();
        }
    }

    public static void SaveToModelFile<T>(this List<T> models, string fileName) where T : class
    {
        var lines = new List<string>();

        foreach (var m in models)
        {
            if (null != m)
            {
                string line = "";

                foreach (var prop in m.GetType().GetProperties())
                {
                    line += $"{prop.GetValue(m)},";
                }

                line = line.Remove(line.Length - 1);
                lines.Add(line);
            }
        }

        File.WriteAllLines(fileName.FullFilePath(), lines);
    }

    private static void SaveToMatchupEntryFile(this List<MatchupEntryModel> matchupEntries)
    {
        var lines = new List<string>();

        foreach (MatchupEntryModel e in matchupEntries)
        {
            //{id,TeamCompeting,Score,ParentMatchup}
            string line = $"{e.Id},{e.TeamCompeting?.Id},{e.Score},{e.ParentMatchup?.Id}";
            lines.Add(line);
        }

        File.WriteAllLines(GlobalConfig.MatchupEntryFile.FullFilePath(), lines);
    }

    private static void SaveToMatchupFile(this List<MatchupModel> matchups)
    {
        var lines = new List<string>();

        foreach (MatchupModel m in matchups)
        {
            //{id,list|of|entries|ids,WinnerId,MatchupRound}
            string line = $"{m.Id},{m.Entries.ConvertIdsToString('|')},{m.Winner?.Id},{m.MatchupRound}";
            lines.Add(line);
        }

        File.WriteAllLines(GlobalConfig.MatchupFile.FullFilePath(), lines);
    }

    public static void SaveToTeamFile(this List<TeamModel> teams, string fileName)
    {
        var lines = new List<string>();

        foreach (TeamModel t in teams)
        {
            string line = $"{t.Id},{t.TeamName},{t.TeamMembers.ConvertIdsToString('|')}";
            lines.Add(line);
        }

        File.WriteAllLines(fileName.FullFilePath(), lines);
    }

    public static void SaveToTournamentFile(this List<TournamentModel> tournaments
        , string fileName)
    {
        // {id,name,entryFee,active,list of teams' ids,list of prizes' ids,list of rounds' ids}
        // {15,CHAMPS,120,1,5|65|41|17|6,7|3|55|23,6^3^12|8^2|8^1

        var lines = new List<string>();

        foreach (TournamentModel t in tournaments)
        {
            var rounds = new List<List<MatchupModel>>();
            foreach (var round in t.Rounds)
            {
                rounds.Add(round.Matchups);
            }

            string line = $"{t.Id},{t.TournamentName},{t.EntryFee},{1}" +
                $",{t.EnteredTeams.ConvertIdsToString('|')}" +
                $",{t.Prizes.ConvertIdsToString('|')}" +
                $",{rounds.ConvertIdsToString('|')}";

            lines.Add(line);
        }

        File.WriteAllLines(fileName.FullFilePath(), lines);
    }

    /// <summary>
    /// Convert a list of data models to a string representation of their Ids.
    /// When provided with unexpected input list, wrong output is produced.
    /// <br></br>
    /// Maximum nesting level = 2
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="models"></param>
    /// <param name="delim"></param>
    /// <returns></returns>
    private static string ConvertIdsToString<T>(this List<T> models, char delim) where T : class
    {
        string output = "";

        if (models.Count == 0)
        {
            return "";
        }

        foreach (var model in models)
        {
            // Very shitty piece of code, but will try
            // TODO - (OPTIONAL) Could be better
            if (typeof(T).Name == typeof(List<>).Name)
            {
                output += $"{(model as List<MatchupModel>)?.ConvertIdsToString('^')}{delim}";
            }
            else if (model is IDataModel) 
            {
                output += $"{(model as IDataModel)?.Id}{delim}";
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        if (output.Length > 0)
            output = output.Remove(output.Length - 1);

        return output;
    }
}
