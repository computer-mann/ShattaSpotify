using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
