using Presentation.Spotify.Dtos;
using SpotifyAPI.Web;

namespace Presentation.Spotify.Extensions
{
    public static class ResponseConversionExtensions
    {
        public static List<SongsInPlaylist> ToSimplePlaylistResponse(this Paging<PlaylistTrack<IPlayableItem>> paging,string playlistId,string playlistName)
        {
            var list = new List<SongsInPlaylist>();
            foreach (var item in paging.Items)
            {
                var track =(FullTrack)item.Track;
                list.Add(new SongsInPlaylist()
                {
                   PlaylistId= playlistId,
                   PlaylistName=playlistName,
                   Songs= new Songs[]
                   {
                       new Songs()
                       {
                           Id=track.Id,
                           Name=track.Name,
                           Artistes=track.Artists.Select(n=>n.Name).ToArray()
                       }
                   }
                });
            }
            return list;
        }
    }
}
