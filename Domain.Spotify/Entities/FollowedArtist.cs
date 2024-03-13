﻿using Spotify.Areas.Auth.Models;
using System.ComponentModel.DataAnnotations;

namespace Spotify.Models
{
    //this will contain the list of artists and my app users care about
    
    public class FollowedArtist: BaseModel
    {
        public MusicNerd User { get; set; }
        public MusicNerd MusicNerdId { get; set; }
        public Artist Artist { get; set; }
        public string ArtistId { get; set; }
    }
}