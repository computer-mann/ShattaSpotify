using Coravel.Invocable;
using Domain.Spotify.Options;
using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using StackExchange.Redis;

namespace Presentation.Spotify.HostedServices
{
    public class RefreshAppTokenCoravelService : IInvocable
    {
        //should go and get the latest tokens every 3580 seconds
        private readonly ILogger<RefreshAppTokenCoravelService> logger;
       private readonly SpotifyAccessConfig _spotifyAccessCredentials;
        private readonly IDatabase _database;


        public RefreshAppTokenCoravelService(ILogger<RefreshAppTokenCoravelService> logger,
            IOptions<SpotifyAccessConfig> options,IConnectionMultiplexer multiplexer)
        {
            this.logger = logger;
            _spotifyAccessCredentials = options.Value;
            _database = multiplexer.GetDatabase(3);
        }

        public async Task Invoke()
        {
            logger.LogInformation("Timed RefreshApp Token Coravel Service running.");
            await SeekHourlyTokens();
        }


        private async Task SeekHourlyTokens()
        {
           var config=SpotifyClientConfig.CreateDefault();
            var request=new ClientCredentialsRequest(_spotifyAccessCredentials.ClientId,_spotifyAccessCredentials.ClientSecret);
            var response=await new OAuthClient(config).RequestToken(request);
        }
    }
}
