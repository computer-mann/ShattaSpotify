using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using StackExchange.Redis;
using StreamNote.Database.Commons.CommonConstants;
using System.Text.Json;

namespace StreamNote.Api.Controllers
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
        /// <summary>
        /// Get the list of artists followed by the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("following/{userId}")]
        public async Task<IActionResult> GetUsersFollowedArtists([FromRoute]string userId,CancellationToken cancellationToken)
        {
            var key = $"{RedisConstants.SpotifyUserKey}:{userId}";
            var userTokenResponse = JsonSerializer.Deserialize<AuthorizationCodeTokenResponse?>((await _database.StringGetAsync(key))!);
            if (userTokenResponse == null)
            {
                return BadRequest();
            }
            var client = new SpotifyClient(userTokenResponse.AccessToken);
            var followedArtistsResponse = await client.Follow.OfCurrentUser(new FollowOfCurrentUserRequest
            {
                Limit = 50,
            },cancellationToken);
            if (followedArtistsResponse == null)
            {
                return NotFound();
            }
            var followedArtists = followedArtistsResponse.Artists.Items!.Select(n=> new { n.Name , n.Id}).ToList();
            return Ok(followedArtists);
        }

    }
}
