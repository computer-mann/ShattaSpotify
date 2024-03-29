﻿namespace Spotify.Models
{
    //this will contain the list of artists my app users care about
    public class Artist: BaseModel
    {
        public Artist(string artistId, string name, string image)
        {
            ArtistId = artistId;
            Name = name;
            Image = image;
        }

        public string ArtistId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
}
