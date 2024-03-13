using FirebaseAdmin;

namespace Spotify.Areas.HostedServices
{
    //short operations
    //how do i guarrantee the notification was sent?
    //yeah, send the message to firebase, google handles the rest
    public class GoogleNotificationHostedService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
