using SpotifyAPI.Web;

namespace Presentation.Spotify.Extensions
{
    public static class ResponseConversionExtensions
    {
        public static List<PlaylistTrack<FullTrack>> ToPlaylistFullTrackList(this Paging<PlaylistTrack<IPlayableItem>> paging)
        {
            var list = new List<PlaylistTrack<FullTrack>>();
            foreach (var item in paging.Items)
            {
                list.Add(new PlaylistTrack<FullTrack>
                {
                    AddedAt = item.AddedAt,
                    AddedBy = item.AddedBy,
                    IsLocal = item.IsLocal,
                    Track = (FullTrack)item.Track
                });
            }
            return list;
        }
    }
}
