using Coravel.Invocable;
using FirebaseAdmin;
using Microsoft.Extensions.Hosting;

namespace Presentation.Spotify.HostedServices
{
    //short operations
    //how do i guarrantee the notification was sent?
    //yeah, send the message to firebase, google handles the rest
    public class GoogleNotificationHostedService : IInvocable
    {
        private readonly ILogger<GoogleNotificationHostedService> _logger;

        public GoogleNotificationHostedService(ILogger<GoogleNotificationHostedService> logger)
        {
            _logger = logger;
        }
        public Task Invoke()
        {
            _logger.LogInformation("GoogleNotificationHostedService");
            return Task.CompletedTask;
        }
    }
}
