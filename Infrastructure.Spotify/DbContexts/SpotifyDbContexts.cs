using Microsoft.EntityFrameworkCore;
using Spotify.Areas.Auth.Models;
using System.Collections.Generic;
using System.IO;

namespace Spotify.Models
{
    public class SpotifyDbContexts: DbContext
    {
        public SpotifyDbContexts(DbContextOptions<SpotifyDbContexts> options) : base(options)
        {

        }
        public DbSet<Song> Songs { get; set; }
        public DbSet<FollowedArtist> FollowedArtists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<PlayList> PlayLists { get; set; }
        public DbSet<Streamer> MusicNerds { get; set; }
        public Artist Artists { get; set; }

    }
}
