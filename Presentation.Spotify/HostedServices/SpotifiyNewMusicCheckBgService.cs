﻿using Microsoft.Extensions.Hosting;

namespace Presentation.Spotify.HostedServices
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
