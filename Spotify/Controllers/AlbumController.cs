using HashidsNet;
using Microsoft.AspNetCore.Mvc;

namespace Spotify.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AlbumController : ControllerBase
    {
        private readonly IHashids hashids;

        public AlbumController(IHashids hashids)
        {
            this.hashids = hashids;
        }
        [HttpGet]
        public IActionResult Index()
        {
            string encoded = hashids.Encode(43);
            return Ok(new { hashedId = encoded });
        }
    }
}
