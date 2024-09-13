namespace Presentation.Spotify.Dtos
{
    public class GetUserPlaylistResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public GetUserPlaylistResponse(string name, string id)
        {
            Name = name;
            Id = id;
        }
    }
}
