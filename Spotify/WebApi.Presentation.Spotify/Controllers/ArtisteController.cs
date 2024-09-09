using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using StackExchange.Redis;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Spotify.Controllers
{
    [ApiController]
    [Route("/artist")]
    public class ArtisteController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IDatabase _database;
        public ArtisteController(IHttpClientFactory httpClientFactory,IConnectionMultiplexer connectionMultiplexer)
        {
            _httpClient = httpClientFactory.CreateClient();
            _database=connectionMultiplexer.GetDatabase();
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Index(string name)
        {
            
            return BadRequest();
        }
    }
}
