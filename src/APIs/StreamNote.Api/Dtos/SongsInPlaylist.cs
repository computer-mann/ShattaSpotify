namespace StreamNote.Api.Dtos
{
    public class SongsInPlaylist
    {
        public string PlaylistId { get; set; } = default!;
        public string PlaylistName { get; set; } = default!;
        public Songs[] Songs { get; set; } = default!;

    }
    public class Songs
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string[] Artistes { get; set; } = default!;
    }
}
