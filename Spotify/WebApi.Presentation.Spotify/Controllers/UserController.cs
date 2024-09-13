using Infrastructure.Spotify.Constants;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Presentation.Spotify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController:ControllerBase
    {
        private readonly IDatabase _database;
        public UserController(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }

        //get user keys
        [HttpGet("keys")]
        public async Task<IActionResult> GetUserKeys()
        {
            
            var keys= await _database.ExecuteAsync("keys",$"{RedisConstants.SpotifyUserKey}*");
            
            return Ok(keys.ToString());
        }
    }
}
