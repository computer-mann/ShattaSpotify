using System.ComponentModel.DataAnnotations;

namespace Spotify.Configuration
{
    public class JwtParamOptions
    {
        [Required]
        public string Issuer { get; set; }
        [Required]
        public string Audience { get; set; }
        [Required]
        [MinLength(32)]
        public string Key { get; set; }
    }
}
