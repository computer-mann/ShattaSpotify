namespace Spotify.Configuration.SpotifyEndPoints
{
    public class SpotifyUrls
    {
        public const string TokenEndpoint = "https://accounts.spotify.com/api/token";
    }
    public class ArtistUrls
    {       
        public const string baseEndpoint = "https://api.spotify.com/v1";
        public const string GetArtistsAlbums = "/artists/{id}/albums";
        public const string GetArtistsRelatedArtists = "/artists/{id}/related-artists";
        public const string GetArtistsTopTracks = "/artists/{id}/top-tracks";
        public const string GetArtist = "/artists/{id}";
        public const string GetSeveralArtists = "/artists";
    }
    public class TrackUrls
    {
        public const string baseEndpoint = "https://api.spotify.com/v1";
        public const string GetSeveralTracks = "/tracks";
        public const string GetTrack = "/tracks/{id}";
    }
    public class AlbumUrls
    {
        public const string baseEndpoint = "https://api.spotify.com/v1";
        public const string GetAlbumTracks = "/albums/{id}/tracks";
        public const string GetAlbum= "/albums/{id}";
        public const string GetSeveralAlbums = "v1/albums";
    }
    public class PlaylistUrls
    {
        public const string baseEndpoint = "https://api.spotify.com/v1";
        public const string RemovePlaylistItems = "/playlists/{playlist_id}/tracks"; //Delete
        public const string GetCurrentUsersPlaylists = "/me/playlists";
        public const string GetPlaylistCoverImage = "/playlists/{playlist_id}/images";
        public const string GetPlaylistItems = "/playlists/{playlist_id}/tracks";
        public const string GetPlaylist = "	/v1/playlists/{playlist_id}";
        public const string GetUsersPlaylists = "/users/{user_id}/playlists";
        public const string AddItemstoPlaylist = "/playlists/{playlist_id}/tracks";
        public const string CreatePlaylist = "/users/{user_id}/playlists";
        public const string AddCustomPlaylistCoverImage = "/playlists/{playlist_id}/images";
        public const string UpdatePlaylistItems = "/playlists/{playlist_id}/tracks";
        public const string ChangePlaylistDetails = "/playlists/{playlist_id}";
        
    }
    public class SearchArtistUrls
    {
        public const string baseEndpoint = "https://api.spotify.com/v1";
        public const string SearchforItem = "https://api.spotify.com/v1/search?q=sarkodie&type=artist&limit=1";
        
    }
    public class UsersProfileUrls
    {
        public const string baseEndpoint = "https://api.spotify.com/v1";
        public const string GetUsersProfile = "/users/{user_id}";
        public const string GetCurrentUsersProfile = "/me";
    }
    public class FollowUrls
    {
        public const string baseEndpoint = "https://api.spotify.com/v1";
        public const string UnfollowArtistsorUsers = "https://api.spotify.com/v1";
        public const string UnfollowPlaylist = "https://api.spotify.com/v1";
        public const string CheckIfUserFollowsArtistsorUsers = "https://api.spotify.com/v1";
        public const string GetFollowedArtists = "https://api.spotify.com/v1";
        public const string CheckifUsersFollowPlaylist = "https://api.spotify.com/v1";
        public const string FollowArtistsorUsers = "https://api.spotify.com/v1";
        public const string FollowPlaylist = "https://api.spotify.com/v1";
        
    }
}
