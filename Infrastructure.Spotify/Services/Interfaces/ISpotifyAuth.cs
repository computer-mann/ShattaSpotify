using Spotify.Configuration.SpotifyEndPoints;

namespace Spotify.Services.Interfaces
{
    public interface ISpotifyAuth
    {
        public Task<TokenResult> GetClientAccessTokenAsync();
        public  Task<string> GetUserRefreshToken();
    }
}
