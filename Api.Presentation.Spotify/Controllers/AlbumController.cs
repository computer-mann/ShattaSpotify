using FirebaseAdmin.Auth;
using Google.Apis.Logging;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Spotify.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AlbumController : ControllerBase
    {
        private readonly IHashids hashids;
        private readonly ILogger<AlbumController> _logger;

        public AlbumController(IHashids hashids, ILogger<AlbumController> logger)
        {
            this.hashids = hashids;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            string encoded = hashids.Encode(43);
            return Ok(new { hashedId = encoded });
        }

        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            UserRecordArgs args = new UserRecordArgs()
            {
                Email = "phnunoo8311@example.com",
                EmailVerified = false,
                PhoneNumber = "+11234567890",
                Password = "secretPassword",
                DisplayName = "John Doe",
                PhotoUrl = "http://www.example.com/12345678/photo.png",
                Disabled = false,
            };
            try
            {
                UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(args);
                return Ok(userRecord);
            }catch(FirebaseAuthException e)
            {
                _logger.LogError(e, "Firebase Exception");
                return BadRequest(new ProblemDetails()
                {
                    Title ="Authentication Problem",
                    Detail=e.Message
                });
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError(e, "Firebase Exception");
                return BadRequest(e.Message);
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e, "Firebase Exception");
                return BadRequest(e.Message);
            }
            

        }
    }
}
