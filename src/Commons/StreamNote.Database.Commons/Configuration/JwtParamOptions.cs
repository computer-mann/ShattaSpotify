using System.ComponentModel.DataAnnotations;

namespace StreamNote.Database.Commons.Configuration
{
    public class JwtParamOptions
    {
        [Required]
        public string Issuer { get; set; } = string.Empty;
        [Required]
        public string Audience { get; set; } = string.Empty;
        [Required]
        [MinLength(32)]
        public string Key { get; set; } = string.Empty;
    }
}
