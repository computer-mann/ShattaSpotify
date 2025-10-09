using SpotifyAPI.Web;

namespace StreamNote.Api.Dtos
{
    public class ArtistesContentDto
    {
        public string AlbumGroup { get; set; } = string.Empty;

        public string AlbumType { get; set; } = string.Empty;

        public string Href { get; set; } = string.Empty;

        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string ReleaseDate { get; set; } = string.Empty;

        public string ReleaseDatePrecision { get; set; } = string.Empty;

        public int TotalTracks { get; set; }

        public string Type { get; set; } = string.Empty;

        public string Uri { get; set; } = string.Empty;
    }
}
