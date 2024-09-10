using System.ComponentModel.DataAnnotations;

namespace Domain.Spotify.Options
{
    public class SpotifyAccessConfig
    {
        [Required]
        public string? RedirectUri { get; set; }
        [Required]
        public string? ClientId { get; set; }
        [Required]
        public string? ClientSecret { get; set; }
    }
}
