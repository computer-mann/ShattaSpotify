namespace Spotify.Models
{
    public class Artist: BaseModel
    {
        public string ArtistId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
}
