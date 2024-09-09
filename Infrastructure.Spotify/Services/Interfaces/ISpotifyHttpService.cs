namespace Spotify.Services.Interfaces
{
    public interface ISpotifyHttpService
    {
        public Task<TokenResult> GetClientAccessTokenAsync();
        public Task<TokenResult> GetStreamerAccessTokenAsync();
        public  Task<string> GetUserRefreshToken();
    }
}
