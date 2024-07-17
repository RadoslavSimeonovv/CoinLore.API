using System.Text.Json.Serialization;

namespace Dynamo_Coinlore.Models
{
    public class TickersResponseModel
    {
        [JsonPropertyName("data")]
        public IEnumerable<TickerModel> Data { get; set; }
    }
}
