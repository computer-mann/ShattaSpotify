using Microsoft.AspNetCore.Mvc;
using Spotify.Configuration.SpotifyEndPoints;
using Spotify.Models.SpotifyApiResponseObjects;
using Spotify.Utilities;
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
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (await _database.StringGetAsync("clientAccessKey")).ToString());
            var result=await _httpClient.GetAsync(SearchArtistUrls.SearchforItem);
            if (result.IsSuccessStatusCode)
            {
                var json = JsonSerializer.Deserialize<GetArtistDTO>(await result.Content.ReadAsStringAsync());
                return Ok(json);
            }
            //add a global cache data exception
            //return the artist name,id, and pic
            return BadRequest(result.ReasonPhrase);
        }
    }
}
