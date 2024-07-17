using System.Text.Json.Serialization;

namespace Dynamo_Coinlore.Models
{
    public class TickerModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("price_usd")]
        public string PriceUsd { get; set; }
    }
}
