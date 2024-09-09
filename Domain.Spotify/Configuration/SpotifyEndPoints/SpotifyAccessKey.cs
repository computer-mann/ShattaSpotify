using System.ComponentModel.DataAnnotations;

namespace Spotify.Configuration.SpotifyEndPoints
{
    public class SpotifyAccessKey
    {
        [Required]
        public string? ClientId { get; set; }
        [Required]
        public string? ClientSecret { get; set; }
        [Required]
        public string? RedirectUri { get; set; }

    }
}
