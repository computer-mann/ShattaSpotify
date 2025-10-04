using Microsoft.EntityFrameworkCore;

namespace StreamNote.Database.Commons.Database
{
    public class AppDbContext: DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        //public DbSet<Song> Songs { get; set; }
        //public DbSet<FollowedArtist> FollowedArtists { get; set; }
        //public DbSet<Album> Albums { get; set; }
        //public DbSet<PlayList> PlayLists { get; set; }
        //public DbSet<Streamer> MusicNerds { get; set; }
        //public Artist Artists { get; set; }

    }
}
