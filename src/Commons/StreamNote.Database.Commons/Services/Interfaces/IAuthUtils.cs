using StreamNote.Database.Commons.Configuration;

namespace Infrastructure.Spotify.Services.Interfaces
{
    public interface IAuthUtils
    {
        public string GenerateJWToken(string email, string display_name, JwtParamOptions jwtParams);
        public string RandomStringGenerator();
    }
}
