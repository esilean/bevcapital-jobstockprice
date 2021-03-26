using Newtonsoft.Json;

namespace BevCapital.StockPrices.Infra.ServiceExtensions.HttpMessageHandler
{
    public class FinnhubAwsModel
    {
        [JsonProperty("FINNHUB_TOKEN")]
        public string FinnhubToken { get; set; }
    }
}
