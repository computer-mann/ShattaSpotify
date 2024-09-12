using Coravel.Invocable;
using StackExchange.Redis;

namespace Presentation.Spotify.HostedServices
{
    public class RefreshUserTokenHostedService : IInvocable
    {
        private readonly IDatabase _database;
        public RefreshUserTokenHostedService(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }
        public Task Invoke()
        {
            throw new NotImplementedException();
        }
    }
}
