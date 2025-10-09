using SpotifyAPI.Web;

namespace StreamNote.Api.Dtos
{
    public class ArtistesAlbumsDto
    {
        public string AlbumGroup { get; set; }

        public string AlbumType { get; set; }

        public string Href { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string ReleaseDate { get; set; }

        public string ReleaseDatePrecision { get; set; }

        public int TotalTracks { get; set; }

        public string Type { get; set; }

        public string Uri { get; set; }
    }
}
