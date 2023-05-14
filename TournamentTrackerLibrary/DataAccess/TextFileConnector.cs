using TournamentTrackerLibrary.DataAccess.TextFileHelpers;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess
{
    public class TextFileConnector : IDataConnection
    {
        // TODO - (OPTIONAL) Take a look at AutoMapper, useful to learn

        private const string PrizesFile = "PrizeModel.csv";

        public PrizeModel CreatePrize(PrizeModel model)
        {
            // Load text file contents and convert them to List<PrizeModel>
            var prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            // Find the max Id 
            int currentId = 1;

            if (prizes.Count > 0)
            {
                currentId = prizes.OrderByDescending(prize => prize.Id).First().Id + 1;
            }

            // Assign the new Id to model
            model.Id = currentId;

            // Add model to prizes list
            prizes.Add(model);

            // Convert prizes list to List<string> and save text to the text file
            prizes.SaveToPrizeModelFile(PrizesFile);

            return model;
        }
    }
}
