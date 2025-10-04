using SpotifyAPI.Web;

namespace Spotify.Services.Interfaces
{
    public interface ISpotifyHttpService
    {
        public Task<ClientCredentialsTokenResponse> GetClientAccessTokenAsync();
        public Task<AuthorizationCodeTokenResponse> GetStreamerAccessTokenAsync();
        public  Task<AuthorizationCodeRefreshResponse> GetUserRefreshToken();
    }
}
