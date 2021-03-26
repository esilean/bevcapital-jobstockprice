using Newtonsoft.Json;

namespace BevCapital.StockPrices.Application.Gateways.PricesServices.Model
{
    public class FinnhubModel
    {
        [JsonProperty("c")]
        public decimal CurrentPrice { get; set; }

        [JsonProperty("h")]
        public decimal High { get; set; }

        [JsonProperty("l")]
        public decimal Low { get; set; }

        [JsonProperty("o")]
        public decimal Open { get; set; }

        [JsonProperty("pc")]
        public decimal PreviousClose { get; set; }

        [JsonProperty("t")]
        public long UnixTimestamp { get; set; }
    }
}
