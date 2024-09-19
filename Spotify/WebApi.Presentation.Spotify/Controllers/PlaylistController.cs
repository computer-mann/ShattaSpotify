using Infrastructure.Spotify.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Spotify.Dtos;
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
        private readonly ILogger<PlaylistController> _logger;

        public PlaylistController(IServiceProvider serviceProvider)
        {
            _database = serviceProvider.GetRequiredService<IConnectionMultiplexer>().GetDatabase();
            _logger = serviceProvider.GetRequiredService<ILogger<PlaylistController>>();
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
            var projection = playlists.Items?.Select(n => new GetUserPlaylistResponse( n.Name, n.Id )).ToList();
            await _database.SendPlaylistToCache(projection).ConfigureAwait(false);
            return Ok(projection);
        }
        //listsongs
        [HttpGet("owned/songs/{userid}/{playlistid}")]
        [ProducesResponseType(typeof(List<FullTrack>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetPlaylistSongs([FromRoute] string userid, [FromRoute] string playlistid)
        {
            var key = $"{RedisConstants.SpotifyUserKey}:{userid}";
            var userTokenResponse = JsonSerializer.Deserialize<AuthorizationCodeTokenResponse?>((await _database.StringGetAsync(key))!);
            if (userTokenResponse == null)
            {
                return StatusCode(StatusCodes.Status424FailedDependency);
            }
            var playlistName= await _database.StringGetAsync($"playlist:{playlistid}");
            var client = new SpotifyClient(userTokenResponse.AccessToken);
            Paging<PlaylistTrack<IPlayableItem>> playlistItems = await client.Playlists.GetItems(playlistid);
            if (playlistItems == null)
            {
                return NotFound();
            }

            return Ok(playlistItems.ToSimplePlaylistResponse(playlistid,playlistName));
        }

        //create playlist
        [HttpPost("owned/{userid}/{playlistName}")]
        public async Task<IActionResult> CreatePlaylist([FromRoute] string userid, [FromBody] string playlistName)
        {
            var key = $"{RedisConstants.SpotifyUserKey}:{userid}";
            var userTokenResponse = JsonSerializer.Deserialize<AuthorizationCodeTokenResponse?>((await _database.StringGetAsync(key))!);
            if (userTokenResponse == null)
            {
                return BadRequest();
            }
            var newPlaylist = new PlaylistCreateRequest(playlistName)
            {
                Public = true,
                Collaborative = false,
                Description = "New playlist created by SpotifyAPI.Web"
            };
            var client = new SpotifyClient(userTokenResponse.AccessToken);
            var playlist = await client.Playlists.Create(userid,newPlaylist);
            if (playlist == null)
            {
                return BadRequest();
            }
            await _database.StringSetAsync($"playlist:{playlist.Id}", playlist.Name);
            return Ok(playlist);
        }
    }
}
