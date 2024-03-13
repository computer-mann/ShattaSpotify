using FirebaseAdmin.Auth;
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

        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            UserRecordArgs args = new UserRecordArgs()
            {
                Email = "phnunoo83@example.com",
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
                return BadRequest(e.Message);
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(e.Message);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            

        }
    }
}
