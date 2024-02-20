using Coravel;
using Coravel.Scheduling.Schedule;
using Spotify.Areas.HostedServices;

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
                schedulers.Schedule(() => Console.WriteLine("Every minute during the week.")).EveryTenSeconds().RunOnceAtStart();

            });
            return app;
        }
    }
}
