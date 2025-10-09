using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using StackExchange.Redis;
using StreamNote.Api.Dtos;
using StreamNote.Database.Commons.CommonConstants;
using StreamNote.Database.Commons.Options;
using System.Text.Json;

namespace StreamNote.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class WebAuthController : ControllerBase
    {
        private readonly ILogger<WebAuthController> _logger;
        private readonly SpotifyAccessConfig _spotifyConfig;
        private readonly IDatabase database;

        public WebAuthController(ILogger<WebAuthController> _logger,
            IOptions<SpotifyAccessConfig> options, IConnectionMultiplexer connection)
        {
            this._logger = _logger;
            _spotifyConfig = options.Value;
            database = connection.GetDatabase();
        }

        [HttpGet("/web/login")]
        public IActionResult SpotifyWebLogin()
        {
            var loginRequest = new LoginRequest(new Uri($"{_spotifyConfig.RedirectUri}"),
                            _spotifyConfig.ClientId!,LoginRequest.ResponseType.Code)
                {
                Scope = [ Scopes.PlaylistReadPrivate,
                                Scopes.UserLibraryModify,Scopes.UserFollowModify,
                                Scopes.PlaylistReadCollaborative,Scopes.UserReadEmail,
                                Scopes.UserReadPrivate,Scopes.UserFollowRead]
                };
            var uri = loginRequest.ToUri();
            return Redirect(uri.ToString());
        }

        [HttpGet("/redirect")]
        public async Task<IActionResult> SpotifyAuthCallback([FromQuery]string? code, string? state, string? error)
        {
            //the code here is the authorization code
            Uri.TryCreate(_spotifyConfig.RedirectUri,UriKind.Absolute,out var uri);
            var userTokenResponse = await new OAuthClient().RequestToken(
               new AuthorizationCodeTokenRequest(_spotifyConfig.ClientId!, _spotifyConfig.ClientSecret!, code!,uri!)
            );
            if (userTokenResponse == null)
            {
                return BadRequest();
            }
            var client= new SpotifyClient(userTokenResponse.AccessToken);
            var user = await client.UserProfile.Current();
            if (user == null)
            {
                return BadRequest();
            }
            var key= $"{RedisConstants.SpotifyUserKey}:{user.Id}";
            var toRedisObject = new UserWithAuthorizationCodeResponse
            {
                AuthorizationCodeTokenResponse = userTokenResponse,
                Email = user.Email!,
            };
            if (await database.StringSetAsync(key, JsonSerializer.Serialize(toRedisObject), TimeSpan.FromMinutes(120)))
            {
                _logger.LogInformation("User data with id={id} logged in and saved to cache", user.Id);
                return Redirect("/swagger");
            }
            _logger.LogInformation("User {@user} logged in", user);
            _logger.LogInformation("UserAppTokenResponse->  {@response}", userTokenResponse);
            return Problem(statusCode:StatusCodes.Status424FailedDependency,
                detail:"Service down on my end",title:"Known Problem");
        }
    }
}
