namespace Spotify.Areas.HostedServices
{
    //long running operations, I am expecting
    public class SpotifiyNewMusicCheckBgService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
