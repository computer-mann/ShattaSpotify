using Domain.Spotify.Configuration;
using Infrastructure.Spotify.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Spotify.Utilities
{
    public class AuthUtils:IAuthUtils
    {    
        public string RandomStringGenerator()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[16];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }
        public string GenerateJWToken(string email, string display_name, JwtParamOptions jwtParams)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtParams.Key));
            var myIssuer = jwtParams.Issuer;
            var myAudience = jwtParams.Audience;
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, display_name),
                }),
                Expires = DateTime.UtcNow.AddDays(14),
                Issuer = myIssuer,
                Audience = myAudience,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
