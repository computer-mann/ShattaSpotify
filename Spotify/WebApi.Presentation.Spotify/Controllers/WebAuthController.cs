using Domain.Spotify.Configuration;
using Domain.Spotify.Database.Entities;
using Domain.Spotify.Options;
using Infrastructure.Spotify.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Spotify.Services.Interfaces;
using StackExchange.Redis;
using System.Collections.Specialized;
using System.Web;

namespace Presentation.Spotify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class WebAuthController : ControllerBase
    {
        private readonly ILogger<WebAuthController> _logger;
        private readonly ISpotifyHttpService spotifyAuth;
        private readonly SpotifyAccessConfig spotifyAccessKey;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConnectionMultiplexer multiplexer;
        private readonly JwtParamOptions jwtParams;
        private readonly UserManager<AudioUser> userManager;
        private readonly IAuthUtils authUtils;

        public WebAuthController(ISpotifyHttpService spotifyAuth, ILogger<WebAuthController> _logger,
            IOptions<SpotifyAccessConfig> options, IHttpClientFactory httpClient, IConnectionMultiplexer connection,
            IOptions<JwtParamOptions> options1, UserManager<AudioUser> userManager, IAuthUtils authUtils)
        {
            this.spotifyAuth = spotifyAuth;
            this._logger = _logger;
            spotifyAccessKey = options.Value;
            _httpClientFactory = httpClient;
            multiplexer = connection;
            jwtParams = options1.Value;
            this.userManager = userManager;
            this.authUtils = authUtils;
        }


        [HttpGet("/web/login")]
        public async Task<IActionResult> SpotifyWebLogin()
        {
            var randomString = authUtils.RandomStringGenerator();
            _logger.LogInformation("Attempting to login user");
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add("response_type", "code");
            queryString.Add("client_id", spotifyAccessKey.ClientId);
            queryString.Add("scope", $"{AuthorizationScopes.UserReadEmail} {AuthorizationScopes.UserReadPrivate} {AuthorizationScopes.PlaylistReadPrivate} {AuthorizationScopes.PlaylistReadCollaborative}");
            queryString.Add("redirect_uri", spotifyAccessKey.RedirectUri);
            queryString.Add("state", randomString);
            var result = await LocalStringSetAsync(randomString, "1", "randomgenerator", TimeSpan.FromSeconds(20));
            if (result)
            {
                return Redirect("https://accounts.spotify.com/authorize?" + queryString.ToString());
            }
            _logger.LogError("Cache is down, couldnt put state random generator in code.");
            return BadRequest(new { message = "Login Failed. Try later" });
        }

        [HttpGet("/web/callback")]
        public async Task<IActionResult> SpotifyAuthCallback(string code, string state, string error)
        {
            return Ok();
        }



        private async Task<bool> LocalStringSetAsync(string key, string value, string keyPadding, TimeSpan duration)
        {
            var db = multiplexer.GetDatabase();
            return await db.StringSetAsync(keyPadding + key, value, duration);
        }
        private async Task<string> LocalStringGetAsync(string key)
        {
            var db = multiplexer.GetDatabase();
            return await db.StringGetAsync(key);
        }


    }
}
