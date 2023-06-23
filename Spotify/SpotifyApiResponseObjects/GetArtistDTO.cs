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
 

    //public class GetArtistDTO
    //{ 
    //    public Artists artists { get; set; }
    //}
    //public class Artists
    //{
    //    public External_Urls external_urls { get; set; }
    //    public Followers followers { get; set; }
    //    public string[] genres { get; set; }
    //    public string href { get; set; }
    //    public string id { get; set; }
    //    public Image[] images { get; set; }
    //    public string name { get; set; }
    //    public int popularity { get; set; }
    //    public string type { get; set; }
    //    public string uri { get; set; }
    //}

    //    public class External_Urls
    //    {
    //        public string spotify { get; set; }
    //    }

    //    public class Followers
    //    {
    //        public object href { get; set; }
    //        public int total { get; set; }
    //    }

    //    public class Image
    //    {
    //        public int height { get; set; }
    //        public string url { get; set; }
    //        public int width { get; set; }
    //    }

}

