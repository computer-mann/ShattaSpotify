using Infrastructure.Spotify.Constants;
using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using StackExchange.Redis;
using System.Text.Json;

namespace Spotify.Controllers
{
    [ApiController]
    [Route("/artist")]
    public class ArtisteController : ControllerBase
    {
        private readonly ILogger<ArtisteController> _logger;
        private readonly IDatabase _database;
        public ArtisteController(IConnectionMultiplexer connectionMultiplexer, ILogger<ArtisteController> logger)
        {

            _database = connectionMultiplexer.GetDatabase();
            _logger = logger;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Index(string name)
        {
            
            return BadRequest();
        }

        [HttpGet("following/{userId}")]
        public async Task<IActionResult> GetUsersFollowedArtists([FromRoute]string userId)
        {
            var key = $"{RedisConstants.SpotifyUserKey}:{userId}";
            var userTokenResponse = JsonSerializer.Deserialize<AuthorizationCodeTokenResponse?>((await _database.StringGetAsync(key))!);
            if (userTokenResponse == null)
            {
                return BadRequest();
            }
            var client = new SpotifyClient(userTokenResponse.AccessToken);
            var followedArtistsResponse = await client.Follow.OfCurrentUser();
            if (followedArtistsResponse == null)
            {
                return BadRequest();
            }
            var followedArtists = followedArtistsResponse.Artists.Items.Select(n=>n.Name).ToList();
            return Ok(followedArtists);
        }

    }
}
