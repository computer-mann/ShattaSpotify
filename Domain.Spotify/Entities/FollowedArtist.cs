using Spotify.Areas.Auth.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Spotify.Models
{
    //this will contain the list of artists and my app users care about
    
    public class FollowedArtist: BaseModel
    {
        public Streamer User { get; set; }
        public Streamer StreamerdId { get; set; }
        public Artist Artist { get; set; }
        public string ArtistId { get; set; }
    }
}
