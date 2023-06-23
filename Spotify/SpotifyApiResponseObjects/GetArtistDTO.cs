using System.Text.Json.Serialization;


namespace Spotify.Models.SpotifyApiResponseObjects
{
        public partial class GetArtistDTO
        {
            [JsonPropertyName("artists")]
            public Artists Artists { get; set; }
        }

        public partial class Artists
        {
            [JsonPropertyName("href")]
            public Uri Href { get; set; }

            [JsonPropertyName("items")]
            public List<Item> Items { get; set; }

            [JsonPropertyName("limit")]
            public long Limit { get; set; }

            [JsonPropertyName("next")]
            public Uri Next { get; set; }

            [JsonPropertyName("offset")]
            public long Offset { get; set; }

            [JsonPropertyName("previous")]
            public object Previous { get; set; }

            [JsonPropertyName("total")]
            public long Total { get; set; }
        }

        public partial class Item
        {
            [JsonPropertyName("external_urls")]
            public ExternalUrls ExternalUrls { get; set; }

            [JsonPropertyName("followers")]
            public Followers Followers { get; set; }

            [JsonPropertyName("genres")]
            public List<string> Genres { get; set; }

            [JsonPropertyName("href")]
            public Uri Href { get; set; }

            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("images")]
            public List<Image> Images { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("popularity")]
            public long Popularity { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("uri")]
            public string Uri { get; set; }
        }

        public partial class ExternalUrls
        {
            [JsonPropertyName("spotify")]
            public Uri Spotify { get; set; }
        }

        public partial class Followers
        {
            [JsonPropertyName("href")]
            public object Href { get; set; }

            [JsonPropertyName("total")]
            public long Total { get; set; }
        }

        public partial class Image
        {
            [JsonPropertyName("height")]
            public long Height { get; set; }

            [JsonPropertyName("url")]
            public Uri Url { get; set; }

            [JsonPropertyName("width")]
            public long Width { get; set; }
        }
 
}

