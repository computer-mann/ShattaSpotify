using HashidsNet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Spotify.Areas.Auth.Models;
using Spotify.Configuration;
using Spotify.Configuration.SpotifyEndPoints;
using Spotify.Services.Interfaces;
using Spotify.Utilities;
using StackExchange.Redis;
using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Web;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentation.Spotify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class WebAuthController : ControllerBase
    {
        private readonly ILogger<WebAuthController> _logger;
        private readonly ISpotifyHttpService spotifyAuth;
        private readonly SpotifyAccessKey spotifyAccessKey;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConnectionMultiplexer multiplexer;
        private readonly JwtParams jwtParams;
        private readonly UserManager<Streamer> userManager;
        private readonly IAuthUtils authUtils;

        public WebAuthController(ISpotifyHttpService spotifyAuth, ILogger<WebAuthController> _logger,
            IOptions<SpotifyAccessKey> options, IHttpClientFactory httpClient, IConnectionMultiplexer connection,
            IOptions<JwtParams> options1, UserManager<Streamer> userManager, IAuthUtils authUtils)
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
            var rString = authUtils.RandomStringGenerator();
            _logger.LogInformation("Attempting to login user");
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add("response_type", "code");
            queryString.Add("client_id", spotifyAccessKey.ClientId);
            queryString.Add("scope", $"{AuthorizationScopes.UserReadEmail} {AuthorizationScopes.UserReadPrivate} {AuthorizationScopes.PlaylistReadPrivate} {AuthorizationScopes.PlaylistReadCollaborative}");
            queryString.Add("redirect_uri", spotifyAccessKey.RedirectUri);
            queryString.Add("state", rString);
            var result = await LocalStringSetAsync(rString, "1", "randomgenerator", TimeSpan.FromSeconds(20));
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
            var returnUrl = "http://localhost:4200";
            if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(await LocalStringGetAsync("randomgenerator" + state)))
            {
                return Redirect($"{returnUrl}/login/error");
            }
            Dictionary<string, string> form = new Dictionary<string, string>
            {
                { "code", code },
                { "state", state },
                { "grant_type", "authorization_code" },
                { "redirect_uri", spotifyAccessKey.RedirectUri }
            };
            var bytes = Encoding.UTF8.GetBytes($"{spotifyAccessKey.ClientId}:{spotifyAccessKey.ClientSecret}");
            var encodedkeys = WebEncoders.Base64UrlEncode(bytes);
            var formContent = new FormUrlEncodedContent(form);
            var http = _httpClientFactory.CreateClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedkeys);
            var spotifyTokenResult = await http.PostAsync("https://accounts.spotify.com/api/token", formContent);
            if (spotifyTokenResult.IsSuccessStatusCode)
            {
                //after getting the spotify token,get the email,username,put in a db and put it in a cache
                var serializedSpotifyToken = JsonSerializer.Deserialize<TokenResult>(await spotifyTokenResult.Content.ReadAsStringAsync());
                var userDataRequest = _httpClientFactory.CreateClient();
                userDataRequest.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", serializedSpotifyToken.AccessToken);
                var profileResult = await userDataRequest.GetAsync("https://api.spotify.com/v1/me");
                if (profileResult.IsSuccessStatusCode)
                {
                    var userInfo = JsonSerializer.Deserialize<UserProfileInfo>(await profileResult.Content.ReadAsStringAsync());
                    var userManagerResult = await userManager.CreateAsync(new Streamer()
                    {
                        Email = userInfo.Email,
                        UserName = userInfo.DisplayName
                    });
                    if (userManagerResult.Succeeded)
                    {
                        if (await LocalStringSetAsync(userInfo.Email, serializedSpotifyToken.AccessToken, "spotifytoken", TimeSpan.FromMinutes(60)))
                        {
                            //Generate the jwt token
                            var myJwToken = authUtils.GenerateJWToken(userInfo.Email, userInfo.DisplayName, jwtParams);
                            return Redirect($"{returnUrl}/login?token=" + myJwToken);
                        }
                        else
                        {
                            _logger.LogError("Could not put user into Cache.");
                            return BadRequest(new { message = "Could not Log in. Something went Wrong" });
                        }
                    }
                    else
                    {
                        _logger.LogError("Could not put user into Database");
                        return BadRequest(new { message = "Could not Log in." });
                    }
                }
                return BadRequest(new { message = "Could not Log in." });
            }
            else
            {
                _logger.LogWarning("{Code} was return with Message {Message}", spotifyTokenResult.StatusCode, spotifyTokenResult.Content.ReadAsStringAsync().Result);
                return BadRequest(new { message = "Could not Log in." });
            }


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
