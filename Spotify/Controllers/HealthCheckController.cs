using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Spotify.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [AllowAnonymous]
    public class HealthCheckController : Controller
    {
        [Route("/health")]
        public IActionResult Index()
        {
            string FilePath = Directory.GetCurrentDirectory() + "/wwwroot/index.html";
            var file = new FileStream(FilePath, FileMode.Open);

            return File(file, "text/html");
        }
    }
}
