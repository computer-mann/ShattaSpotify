using Coravel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Presentation.Spotify.HostedServices;
using System.Globalization;

namespace Spotify.CustomMiddlewares
{
    public class SchedulingServiceMidddleware
    {

    }
    public static class SchedulingServiceMidddlewareExtension
    {
        public static IApplicationBuilder UseCoravelSchedulingServices(this IApplicationBuilder app)
        {
            var provider = app.ApplicationServices;
            provider.UseScheduler(schedulers =>
            {
                schedulers.Schedule<RefreshAppTokenCoravelService>().Hourly().RunOnceAtStart();
                schedulers.Schedule(() => Console.WriteLine("Every second of the app running.")).EveryTenSeconds().RunOnceAtStart();

            });
            return app;
        }
        
    }
}