using Microsoft.AspNetCore.Mvc;

namespace Spotify.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
    public class TrackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
