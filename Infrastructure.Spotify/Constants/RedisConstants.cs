﻿namespace Infrastructure.Spotify.Constants
{
    public class RedisConstants
    {
        public const string SpotifyUserKey = "SpotifyUserToken";
        public const string SpotifyAppToken= "SpotifyAppToken";
        public static readonly TimeSpan SendPlaylistToCacheTimeSpan = TimeSpan.FromDays(3);
    }
}
