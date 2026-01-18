using System.Text.Json.Serialization;

namespace MoneyMagic.GraphQL.Binance
{
    public class OrderBook
    {
        [JsonPropertyName("lastUpdateId")]
        public long LastUpdateId { get; set; }

        [JsonPropertyName("bids")]
        public List<List<string>> Bids { get; set; } = [];

        [JsonPropertyName("asks")]
        public List<List<string>> Asks { get; set; } = [];
    }
}
