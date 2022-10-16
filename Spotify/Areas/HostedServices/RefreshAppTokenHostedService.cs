using HashidsNet;
using StackExchange.Redis;
using Coravel.Invocable;
using Spotify.Services.Interfaces;

namespace Spotify.Areas.HostedServices
{
    public class RefreshAppTokenHostedService: IInvocable
    {
        //should go and get the latest tokens every 3580 seconds
        private readonly ILogger<RefreshAppTokenHostedService> logger;
        private readonly ISpotifyAuth auth;
        

        public RefreshAppTokenHostedService(ILogger<RefreshAppTokenHostedService> logger, 
            ISpotifyAuth auth)
        {
            this.logger = logger;
            
            this.auth = auth;
        }

        public async Task Invoke()
        {
            logger.LogInformation("Timed RefreshApp Token Hosted Service running.");
            await SeekHourlyTokens();
        }


        async Task SeekHourlyTokens()
        {
            var result=await auth.GetClientAccessTokenAsync();
            if (result.Success)
            {
                logger.LogInformation("Successfully put client access token into cache");
            }
            else
            {
                logger.LogWarning("Something bad happened");
            }
        }
    }
}
