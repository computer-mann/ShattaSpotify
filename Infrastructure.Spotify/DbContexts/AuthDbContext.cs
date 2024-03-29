﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Spotify.Areas.Auth.Models.DbContext
{
    public class AuthDbContext : IdentityDbContext<Streamer>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options):base(options)
        {

        }
    }
}
