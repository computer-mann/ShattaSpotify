using System.ComponentModel.DataAnnotations;

namespace StreamNote.Database.Commons.Configuration
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
