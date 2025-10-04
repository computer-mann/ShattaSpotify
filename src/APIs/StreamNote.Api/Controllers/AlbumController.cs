using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StreamNote.Database.Commons.Options;

namespace StreamNote.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AlbumController : ControllerBase
    {
        private readonly ILogger<AlbumController> _logger;
        private readonly SpotifyAccessConfig _spotifyAccessConfig;

        public AlbumController(ILogger<AlbumController> logger,IOptions<SpotifyAccessConfig> options)
        {
            _logger = logger;
            _spotifyAccessConfig = options.Value;
        }
        [HttpGet]
        public IActionResult Index()
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
