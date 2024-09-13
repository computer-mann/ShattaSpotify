using Coravel.Invocable;
using Domain.Spotify.Options;
using Infrastructure.Spotify.Constants;
using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using StackExchange.Redis;
using System.Text.Json;

namespace Presentation.Spotify.HostedServices
{
    public class RefreshUserTokenHostedService : IInvocable
    {
        private readonly IDatabase _database;
        private readonly SpotifyAccessConfig _spotifyConfig;
        private readonly ILogger<RefreshUserTokenHostedService> _logger;
        public RefreshUserTokenHostedService(IConnectionMultiplexer connectionMultiplexer, IOptions<SpotifyAccessConfig> options, ILogger<RefreshUserTokenHostedService> logger)
        {
            _database = connectionMultiplexer.GetDatabase();
            _spotifyConfig = options.Value;
            _logger = logger;
        }
        public async Task Invoke()
        {
            _logger.LogInformation("Refreshing user tokens:=>");
            var redisResult = await _database.ExecuteAsync("keys", $"{RedisConstants.SpotifyUserKey}*");
            var keys = new List<string>();
            for (int i = 0; i < redisResult.Length; i++)
            {
                var userKey = (RedisValue)redisResult[i];
                if (userKey.HasValue)
                {
                    keys.Add(userKey!);
                }
            }
            if (keys.Any())
            {
                _logger.LogInformation("Found {count} user keys",keys.Count);
                keys.ForEach(async key =>
                {
                    var user = await _database.StringGetAsync(key);
                    var userToken = JsonSerializer.Deserialize<AuthorizationCodeTokenResponse>(user!);
                    if (userToken != null && DateTime.UtcNow.Subtract(userToken.CreatedAt) >= TimeSpan.FromHours(1))
                    {
                        var newAuthTokenResponse = await new OAuthClient().RequestToken(
                        new AuthorizationCodeRefreshRequest(_spotifyConfig.ClientId!, _spotifyConfig.ClientSecret!, userToken!.RefreshToken)
                        );
                        if (newAuthTokenResponse != null)
                        {
                            await _database.StringSetAsync(key, JsonSerializer.Serialize(newAuthTokenResponse),TimeSpan.FromHours(2));
                            _logger.LogInformation($"Refreshed token for user {key}");
                        }
                    }                    
                });
            }
        }
    }
}
