using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using StackExchange.Redis;
using StreamNote.Api.Dtos;
using StreamNote.Api.Extensions;
using StreamNote.Database.Commons.CommonConstants;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StreamNote.Api.Controllers
{
    [Route("[controller]")]
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
            var redisValue = await _database.StringGetAsync(key);
            if(!redisValue.HasValue)
            {
                _logger.LogError("User token not found in cache for user {userid}", userid);
                return NotFound();
            }
            var userTokenResponse = JsonSerializer.Deserialize<UserWithAuthorizationCodeResponse?>(redisValue.ToString());
            //if (userTokenResponse == null)
            //{
            //    _logger.LogError("User token not found in cache for user {userid}", userid);
            //    return Problem();
            //}
            var client = new SpotifyClient(userTokenResponse!.AuthorizationCodeTokenResponse.AccessToken);
            var playlists = await client.Playlists.CurrentUsers();
            if (playlists == null)
            {
                return BadRequest();
            }
            var projection = playlists.Items?.Select(n => new GetUserPlaylistResponse( n.Name!, n.Id! )).ToList();
            await _database.SendPlaylistToCache(projection!).ConfigureAwait(false);
            return Ok(projection);
        }
        //listsongs
        [HttpGet("owned/songs/{userid}/{playlistid}")]
        [ProducesResponseType(typeof(List<FullTrack>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetPlaylistSongs([FromRoute] string userid, [FromRoute] string playlistid)
        {
            var key = $"{RedisConstants.SpotifyUserKey}:{userid}";
            var userToken = await _database.StringGetAsync(key);
            var userTokenResponse = JsonSerializer.Deserialize<UserWithAuthorizationCodeResponse?>(userToken.ToString());
            if (userTokenResponse == null)
            {
                return StatusCode(StatusCodes.Status424FailedDependency);
            }
            var playlistName= await _database.StringGetAsync($"playlist:{playlistid}");
            var client = new SpotifyClient(userTokenResponse.AuthorizationCodeTokenResponse.AccessToken);
            Paging<PlaylistTrack<IPlayableItem>> playlistItems = await client.Playlists.GetItems(playlistid);
            if (playlistItems == null || !playlistItems.Items!.Any())
            {
                return NotFound();
            }

            return Ok(playlistItems.ToSimplePlaylistResponse(playlistid!, playlistName!));
        }

        //create playlist
        [HttpPost("owned/{userid}/{playlistName}")]
        public async Task<IActionResult> CreatePlaylist([FromRoute] string userid, [FromBody] string playlistName)
        {
            var key = $"{RedisConstants.SpotifyUserKey}:{userid}";
            var userToken = await _database.StringGetAsync(key);
            var userTokenResponse = JsonSerializer.Deserialize<AuthorizationCodeTokenResponse?>(userToken.ToString());
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
