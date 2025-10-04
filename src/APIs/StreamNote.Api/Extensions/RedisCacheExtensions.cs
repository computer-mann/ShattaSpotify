using Presentation.Spotify.Dtos;
using StackExchange.Redis;
using System.Text.Json;

namespace Presentation.Spotify.Extensions
{
    public static class RedisCacheExtensions
    {
        public static async Task SendPlaylistToCache(this IDatabase database,List<GetUserPlaylistResponse> playlista)
        {
            foreach (var item in playlista)
            {
                var key = "playlist:" + item.Id;
                await database.StringSetAsync(key, item.Name, TimeSpan.FromDays(3));
            }
           
        }
    }
}
