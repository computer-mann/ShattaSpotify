﻿namespace Spotify.SpotifyApiResponseObjects
{
    public class GetAlbumTracksDto
    {
        public string href { get; set; }
        public Item[] items { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public string previous { get; set; }
        public int total { get; set; }

        public class Item
        {
            public Artist[] artists { get; set; }
            public int disc_number { get; set; }
            public int duration_ms { get; set; }
            public bool _explicit { get; set; }
            public External_Urls external_urls { get; set; }
            public string href { get; set; }
            public string id { get; set; }
            public bool is_local { get; set; }
            public bool is_playable { get; set; }
            public string name { get; set; }
            public string preview_url { get; set; }
            public int track_number { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }

        public class External_Urls
        {
            public string spotify { get; set; }
        }

        public class Artist
        {
            public External_Urls external_urls { get; set; }
            public string href { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }

  

    }
}
