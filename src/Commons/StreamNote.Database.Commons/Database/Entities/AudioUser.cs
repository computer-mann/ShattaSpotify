using Microsoft.AspNetCore.Identity;
using UuidExtensions;

namespace StreamNote.Database.Commons.Database.Entities
{
    public class AudioUser:IdentityUser<string>
    {
        public AudioUser()
        {
            Id = Uuid7.Id25();
        }
    }
}
