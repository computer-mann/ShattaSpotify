using Infrastructure.Spotify.Constants;
using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using StackExchange.Redis;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spotify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly IDatabase _database;

        public PlaylistController(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }
        
        [HttpGet("owned/{userid}")]
        public async Task<IActionResult> GetUserPlaylist([FromRoute]string userid)
        {
            var key = $"{RedisConstants.SpotifyUserKey}:{userid}";
            var userTokenResponse = JsonSerializer.Deserialize<AuthorizationCodeTokenResponse?>((await _database.StringGetAsync(key))!);
            if (userTokenResponse == null)
            {
                return BadRequest();
            }
            var client = new SpotifyClient(userTokenResponse.AccessToken);
            var playlists = await client.Playlists.CurrentUsers();
            if (playlists == null)
            {
                return BadRequest();
            }
            var projection = playlists.Items?.Select(n => new { n.Name, n.Id }).ToList();
            return Ok(projection);
        }
        //listsongs
        [HttpGet("songs/{userid}/{playlistid}")]
        public async Task<IActionResult> GetPlaylistSongs([FromRoute]string userid,[FromRoute] string playlistid)
        {
            var key = $"{RedisConstants.SpotifyUserKey}:{userid}";
            var userTokenResponse = JsonSerializer.Deserialize<AuthorizationCodeTokenResponse?>((await _database.StringGetAsync(key))!);
            if (userTokenResponse == null)
            {
                return BadRequest();
            }
            var client = new SpotifyClient(userTokenResponse.AccessToken);
            var playlistItems = await client.Playlists.GetItems(playlistid);
            if (playlistItems == null)
            {
                return BadRequest();
            }
            var projection = playlistItems?.Items?.Select(n => new { n.Track }).Select(t => t as FullTrack ).ToList();
            return Ok(projection);
        }
    }
}
