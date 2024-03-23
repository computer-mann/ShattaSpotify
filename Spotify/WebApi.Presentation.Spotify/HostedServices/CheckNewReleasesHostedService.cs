using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Presentation.Spotify.HostedServices
{
    /*
     * to get new releases, get the artist's song 
     * then get 
     * payload should have a release date
     */
    public class CheckNewReleasesHostedService : IInvocable
    {
        private readonly ILogger<CheckNewReleasesHostedService> logger;
        private readonly IConnectionMultiplexer redis;

        public CheckNewReleasesHostedService(ILogger<CheckNewReleasesHostedService> logger, IConnectionMultiplexer redis)
        {
            this.logger = logger;
            this.redis = redis;
        }

        public Task Invoke()
        {
            logger.LogInformation("CheckNewReleasesHostedService");
            return Task.CompletedTask;
        }
    }
}
