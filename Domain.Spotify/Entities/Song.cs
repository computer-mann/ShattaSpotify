namespace Spotify.Models
{
    public class Song: BaseModel
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
