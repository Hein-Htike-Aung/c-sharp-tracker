using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class PriceModel
    {
        public int Id { get; set; }
        public int PlaceNumber { get; set; }
        public string PlaceName { get; set; }
        public decimal PriceAmount { get; set; }
        public double PricePercentage { get; set; }

        public PriceModel()
        {

        }

        public PriceModel(string placeNumber, string placeName, string priceAmount, string pricePercentage)
        {
            int placeNumberValue = 0;
            int.TryParse(placeNumber, out placeNumberValue);
            decimal priceAmountValue = 0;
            decimal.TryParse(priceAmount, out priceAmountValue);
            double pricePercentageValue = 0;
            double.TryParse(pricePercentage, out pricePercentageValue);

            this.PlaceName = placeName;
            this.PlaceNumber = placeNumberValue;
            this.PriceAmount = priceAmountValue;
            this.PricePercentage = pricePercentageValue;
        }
    }
}
