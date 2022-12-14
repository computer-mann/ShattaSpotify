using HashidsNet;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Spotify.Configuration.SpotifyEndPoints;
using Spotify.Services.Interfaces;
using StackExchange.Redis;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Spotify.Services
{
    //this will have the non-user(client) token seek
    // the authorization flows

    public class SpotifyAuthHttpService : ISpotifyAuth
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IHashids hashid;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly SpotifyAccessKey spotifyAccessKey;
        private readonly ILogger<SpotifyAuthHttpService> logger;

        public SpotifyAuthHttpService(IHttpClientFactory httpClientFactory,IConnectionMultiplexer redis,
            IHashids hashids,IOptions<SpotifyAccessKey> options,ILogger<SpotifyAuthHttpService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            _redis = redis;
            hashid = hashids;
            spotifyAccessKey = options.Value;
            this.logger = logger;
        }
        public async Task<TokenResult> GetClientAccessTokenAsync()
        {
            var db = _redis.GetDatabase();
            var accessKey = await db.StringGetAsync("clientAccessKey");
            if (!accessKey.IsNullOrEmpty)
            {
                return new TokenResult() { AccessToken=accessKey};
            }
            var httpContent = new FormUrlEncodedContent(new Dictionary<string, string> { { "grant_type", "client_credentials" } });
            var http = httpClientFactory.CreateClient();
            var bytes = Encoding.UTF8.GetBytes($"{spotifyAccessKey.ClientId}:{spotifyAccessKey.ClientSecret}");
            var encodedkeys = WebEncoders.Base64UrlEncode(bytes);
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedkeys);

            var result = await http.PostAsync(SpotifyUrls.TokenEndpoint, httpContent);
            if (result.IsSuccessStatusCode)
            {
                var tokenResult = JsonSerializer.Deserialize<TokenResult>(await result.Content.ReadAsStringAsync());
                var inCache = await db.StringSetAsync("clientAccessKey", tokenResult.AccessToken, TimeSpan.FromSeconds(tokenResult.ExpiresIn));
                if (inCache)
                {
                    return tokenResult;
                }
                else
                {
                    logger.LogWarning("Client access token was not put in cache");
                    return tokenResult;
                }
            }
            return new TokenResult() { Success=false};
        }

       
        public Task<string> GetUserRefreshToken()
        {
            throw new NotImplementedException();
        }

        
    }
}
