using Microsoft.AspNetCore.Mvc;


namespace StreamNote.Api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("native")]
    public class AndroidAuthController : Controller
    {
        public AndroidAuthController()
        {

        }

        [Route("login")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("callback")]
        public IActionResult Callback()
        {
            return RedirectToAction("Index");
        }

    }
}
