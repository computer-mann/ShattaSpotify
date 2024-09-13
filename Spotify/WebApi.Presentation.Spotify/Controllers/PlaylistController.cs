using Infrastructure.Spotify.Constants;
using Microsoft.AspNetCore.Mvc;
using Presentation.Spotify.Extensions;
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
        //logger
        private readonly ILogger<PlaylistController> _logger;

        public PlaylistController(IConnectionMultiplexer connectionMultiplexer, ILogger<PlaylistController> logger)
        {
            _database = connectionMultiplexer.GetDatabase();
            _logger = logger;
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
        public async Task<ActionResult> GetPlaylistSongs([FromRoute] string userid, [FromRoute] string playlistid)
        {
            var key = $"{RedisConstants.SpotifyUserKey}:{userid}";
            var userTokenResponse = JsonSerializer.Deserialize<AuthorizationCodeTokenResponse?>((await _database.StringGetAsync(key))!);
            if (userTokenResponse == null)
            {
                return BadRequest();
            }
            var client = new SpotifyClient(userTokenResponse.AccessToken);
            Paging<PlaylistTrack<IPlayableItem>> playlistItems = await client.Playlists.GetItems(playlistid);
            if (playlistItems == null)
            {
                return BadRequest();
            }

            return Ok(playlistItems.ToPlaylistFullTrackList());
        }
    }
}
