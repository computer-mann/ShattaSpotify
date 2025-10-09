using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using StreamNote.Database.Commons.Options;

namespace StreamNote.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AlbumController : ControllerBase
    {
        private readonly ILogger<AlbumController> _logger;
        private readonly SpotifyAccessConfig _spotifyAccessConfig;
        private readonly IDatabase _database;

        public AlbumController(ILogger<AlbumController> logger,
            IOptions<SpotifyAccessConfig> options, IConnectionMultiplexer connectionMultiplexer)
        {
            _logger = logger;
            _spotifyAccessConfig = options.Value;
            _database = connectionMultiplexer.GetDatabase();
        }
        [HttpGet]
        public IActionResult GetArtistAlbums()
        {

            return Ok();
        }

        [HttpGet]
        public IActionResult GetOptionsConfig()
        {
            return Ok(_spotifyAccessConfig);
        }

    }
    
}
