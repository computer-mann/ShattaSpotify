namespace Spotify.Models
{
    public class PlayList: BaseModel
    {
        public string Name { get; set; }
        public int UserId { get; set; }
    }
}
