using StreamNote.Api.HostedServices;

namespace StreamNote.Api.CustomMiddlewares
{

    public static class ApplicationBuilderMidddlewareExtension
    {
        public static IApplicationBuilder UseCoravelSchedulingServices(this IApplicationBuilder app)
        {
            var provider = app.ApplicationServices;
            provider.UseScheduler(schedulers =>
            {
                schedulers.Schedule<RefreshAppTokenCoravelService>().Hourly().RunOnceAtStart();
                schedulers.Schedule<RefreshUserTokenHostedService>().Hourly().RunOnceAtStart();
                //schedulers.Schedule<WeeklyDbSongClearanceHostedService>().EverySeconds(14).RunOnceAtStart();
                //schedulers.Schedule<GoogleNotificationHostedService>().EverySeconds(13).RunOnceAtStart();
                //schedulers.Schedule<CheckNewReleasesHostedService>().EverySeconds(12).RunOnceAtStart();
                //schedulers.Schedule(() => Console.WriteLine("Every second of the app running.")).EveryTenSeconds().RunOnceAtStart();

            });
            return app;
        }
        
    }
}