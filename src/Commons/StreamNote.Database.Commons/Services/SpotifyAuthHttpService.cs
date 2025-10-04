using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spotify.Services.Interfaces;
using SpotifyAPI.Web;
using StackExchange.Redis;

namespace Infrastructure.Spotify.Services
{
    //this will have the authorization flows for getting both client and user tokens

    public class SpotifyHttpService : ISpotifyHttpService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IHashids hashid;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly SpotifyAccessConfig spotifyAccessKey; //i should probably make this a singleton
        private readonly ILogger<SpotifyHttpService> logger;

        public SpotifyHttpService(IHttpClientFactory httpClientFactory,IConnectionMultiplexer redis,
            IHashids hashids,IOptions<SpotifyAccessConfig> options,ILogger<SpotifyHttpService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            _redis = redis;
            hashid = hashids;
            spotifyAccessKey = options.Value;
            this.logger = logger;
        }

        public Task<ClientCredentialsTokenResponse> GetClientAccessTokenAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AuthorizationCodeTokenResponse> GetStreamerAccessTokenAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AuthorizationCodeRefreshResponse> GetUserRefreshToken()
        {
            throw new NotImplementedException();
        }

        //public async Task<TokenResult> GetClientAccessTokenAsync()
        //{
        //    var db = _redis.GetDatabase();
        //    const string RedisKey = "clientAccessKey";
        //    var accessKey = await db.StringGetAsync(RedisKey);
        //    if (!accessKey.IsNullOrEmpty)
        //    {
        //        logger.LogInformation("Client Access token has not expired. Skipping new request to Spotify");
        //        return new TokenResult() { AccessToken=accessKey};
        //    }
        //    var formUrlEncodedContent = new FormUrlEncodedContent(new Dictionary<string, string> { { "grant_type", "client_credentials" } });
        //    var http = httpClientFactory.CreateClient();
        //    var bytes = Encoding.UTF8.GetBytes($"{spotifyAccessKey.ClientId}:{spotifyAccessKey.ClientSecret}");
        //    var encodedkeys = WebEncoders.Base64UrlEncode(bytes);
        //    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedkeys);

        //    var result = await http.PostAsync(SpotifyUrls.TokenEndpoint, formUrlEncodedContent);
        //    if (result.IsSuccessStatusCode)
        //    {
        //        var tokenResult = JsonSerializer.Deserialize<TokenResult>(await result.Content.ReadAsStringAsync());
        //        logger.LogInformation("the token lifetime is : {token}",tokenResult.ExpiresIn);
        //        var inCache = await db.StringSetAsync(RedisKey, tokenResult.AccessToken, TimeSpan.FromSeconds(tokenResult.ExpiresIn));
        //        if (inCache)
        //        {
        //            return tokenResult;
        //        }
        //        else
        //        {
        //            logger.LogWarning("Client access token was not put in cache");
        //            return tokenResult;
        //        }
        //    }
        //    logger.LogCritical("Token request not successful: {message}",result.ToString());
        //    return new TokenResult() { Success=false};
        //}




    }
}
