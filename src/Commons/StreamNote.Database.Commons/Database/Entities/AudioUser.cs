using Microsoft.AspNetCore.Identity;
using UuidExtensions;

namespace Domain.Spotify.Database.Entities
{
    public class AudioUser:IdentityUser<string>
    {
        public AudioUser()
        {
            Id = Uuid7.Id25();
        }
    }
}
