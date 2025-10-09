using SpotifyAPI.Web;

namespace StreamNote.Api.Dtos
{
    public class UserWithAuthorizationCodeResponse 
    {
        public AuthorizationCodeTokenResponse AuthorizationCodeTokenResponse { get; set; } = null!;
        public string Email { get; set; } = string.Empty;
    }
}
