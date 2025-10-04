using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace StreamNote.Api.Controllers
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
            
           
            return Ok();
        }
    }
}
