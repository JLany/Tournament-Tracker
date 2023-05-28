namespace TournamentTrackerLibrary.Models
{
    public class PrizeModel : IDataModel
    {
        public int Id { get; set; }
        public int PlaceNumber { get; set; }
        public string? PlaceName { get; set; }
        public decimal PrizeAmount { get; set; }
        public int PrizePercentage { get; set; }

        public PrizeModel() { }

        /// <summary>
        /// Fail tolerant constructor
        /// </summary>
        /// <param name="placeNumber"></param>
        /// <param name="placeName"></param>
        /// <param name="prizeAmount"></param>
        /// <param name="prizePercentage"></param>
        public PrizeModel(string placeNumber, string placeName, string prizeAmount, string prizePercentage)
        {
            int.TryParse(placeNumber, out int placeNumberValue);
            PlaceNumber = placeNumberValue;

            PlaceName = placeName;

            decimal.TryParse(prizeAmount, out decimal prizeAmountValue);
            PrizeAmount = prizeAmountValue;

            int.TryParse(prizePercentage, out int prizePercentageValue);
            PrizePercentage = prizePercentageValue;
        }
    }
}
