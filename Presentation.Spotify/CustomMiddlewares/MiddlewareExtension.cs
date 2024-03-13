using Coravel;
using Coravel.Scheduling.Schedule;
using Presentation.Spotify.HostedServices;

namespace Spotify.CustomMiddlewares
{
    public static class MiddlewareExtension
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
