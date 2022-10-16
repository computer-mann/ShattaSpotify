using Microsoft.AspNetCore.Mvc;

namespace Spotify.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
    public class ArtisteController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
