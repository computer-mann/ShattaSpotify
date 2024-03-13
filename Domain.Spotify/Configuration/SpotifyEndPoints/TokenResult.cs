using System.Text.Json.Serialization;

namespace Spotify.Configuration.SpotifyEndPoints
{
    public class TokenResult
    {

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public long ExpiresIn { get; set; }
        [JsonIgnore]
        public bool Success = true;

    }
}
