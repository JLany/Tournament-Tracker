using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess
{
    public class TextFileConnector : IDataConnection
    {
        public PrizeModel CreatePrize(PrizeModel model)
        {
            // TODO - Implement the actual text file storage logic

            model.Id = 1;

            return model;
        }
    }
}
