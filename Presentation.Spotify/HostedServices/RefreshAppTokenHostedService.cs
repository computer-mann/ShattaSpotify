using HashidsNet;
using StackExchange.Redis;
using Coravel.Invocable;
using Spotify.Services.Interfaces;

namespace Presentation.Spotify.HostedServices
{
    public class RefreshAppTokenCoravelService : IInvocable
    {
        //should go and get the latest tokens every 3580 seconds
        private readonly ILogger<RefreshAppTokenCoravelService> logger;
        private readonly ISpotifyAuth auth;


        public RefreshAppTokenCoravelService(ILogger<RefreshAppTokenCoravelService> logger,
            ISpotifyAuth auth)
        {
            this.logger = logger;

            this.auth = auth;
        }

        public async Task Invoke()
        {
            logger.LogInformation("Timed RefreshApp Token Coravel Service running.");
            await SeekHourlyTokens();
        }


        private async Task SeekHourlyTokens()
        {
            var result = await auth.GetClientAccessTokenAsync();
            if (result.Success)
            {
                logger.LogInformation("Successfully put client access token into cache");
            }
            else
            {
                //make better logs
                logger.LogWarning("Something bad happened:");
            }
        }
    }
}
