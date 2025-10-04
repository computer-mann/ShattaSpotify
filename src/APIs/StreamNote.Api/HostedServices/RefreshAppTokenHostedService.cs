using Coravel.Invocable;
using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using StackExchange.Redis;
using StreamNote.Database.Commons.CommonConstants;
using StreamNote.Database.Commons.Options;

namespace StreamNote.Api.HostedServices
{
    public class RefreshAppTokenCoravelService : IInvocable
    {
        //should go and get the latest tokens every 3580 seconds
        private readonly ILogger<RefreshAppTokenCoravelService> logger;
        private readonly SpotifyAccessConfig _spotifyAccessCredentials;
        private readonly IDatabase _database;


        public RefreshAppTokenCoravelService(ILogger<RefreshAppTokenCoravelService> logger,
            IOptions<SpotifyAccessConfig> options, IConnectionMultiplexer multiplexer)
        {
            this.logger = logger;
            _spotifyAccessCredentials = options.Value;
            _database = multiplexer.GetDatabase();
        }

        public async Task Invoke()
        {
            logger.LogInformation("Timed RefreshApp Token Coravel Service running.");
            await SeekHourlyTokens();
            logger.LogInformation("Timed RefreshApp Token Coravel Service finished.");
        }


        private async Task SeekHourlyTokens()
        {
            var token = await _database.StringGetAsync("SpotifyAppToken");
            if (token.HasValue)
            {
                logger.LogInformation("Spotify App Token already exists,background service wont run");
                return;
            }
            var config = SpotifyClientConfig.CreateDefault();
            var request = new ClientCredentialsRequest(_spotifyAccessCredentials.ClientId!, _spotifyAccessCredentials.ClientSecret!);
            var response = await new OAuthClient(config).RequestToken(request);
            if (response != null)
            {
                await _database.StringSetAsync(RedisConstants.SpotifyAppToken, response.AccessToken, TimeSpan.FromMinutes(59));
                logger.LogInformation("[RefreshAppTokenCoravelService:SeekHourlyTokens] Spotify App Token Refreshed");
            }
            else
            {
                logger.LogError("Failed to get Spotify App Token");
            }
        }
    }
}
