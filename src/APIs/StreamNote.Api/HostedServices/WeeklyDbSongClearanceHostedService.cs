using Coravel.Invocable;

namespace StreamNote.Api.HostedServices
{
    public class WeeklyDbSongClearanceHostedService : IInvocable
    {
        private readonly ILogger<WeeklyDbSongClearanceHostedService> _logger;

        public WeeklyDbSongClearanceHostedService(ILogger<WeeklyDbSongClearanceHostedService> logger)
        {
            _logger = logger;
        }
        public Task Invoke()
        {
            _logger.LogInformation("WeeklyDbSongClearanceHostedService");
            return Task.CompletedTask;
        }
    }
}
