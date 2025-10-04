using Coravel.Invocable;

namespace StreamNote.Api.HostedServices
{
    /*
     * to get new releases, get the artist's song 
     * then get 
     * payload should have a release date
     */
    public class CheckNewReleasesHostedService : IInvocable
    {
        private readonly ILogger<CheckNewReleasesHostedService> logger;
       // private readonly IConnectionMultiplexer redis;

        public CheckNewReleasesHostedService(ILogger<CheckNewReleasesHostedService> logger)
        {
            this.logger = logger;
            
        }

        public Task Invoke()
        {
            logger.LogInformation("CheckNewReleasesHostedService");
            return Task.CompletedTask;
        }
    }
}
