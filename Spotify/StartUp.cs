using HashidsNet;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Spotify.Areas.HostedServices;
using Spotify.Configuration;
using Spotify.Configuration.SpotifyEndPoints;
using Spotify.Models;
using StackExchange.Redis;
using Pomelo.EntityFrameworkCore.MySql;
using Spotify.Areas.Auth.Models.DbContext;
using Spotify.Areas.Auth.Models;
using Coravel;
using Spotify.Services.Interfaces;
using Spotify.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Spotify
{
    public class StartUp
    {
        

        public IConfiguration Configuration { get; set; }

        public StartUp(IConfiguration configuration)
        {
            this.Configuration = configuration;
            
        }

        public void ConfigureServices(IServiceCollection services,IWebHostBuilder webHost)
        {
            webHost.ConfigureKestrel(options =>
            {
                options.AddServerHeader = false;
            });
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHttpClient();
            services.Configure<SpotifyAccessKey>(Configuration.GetSection("Spotify"));
            services.Configure<JwtParams>(Configuration.GetSection("JWT"));
            services.AddSingleton<IHashids>(new Hashids(Configuration.GetSection("HashId:Salt").Value, 5));
            var multiplexer = ConnectionMultiplexer.Connect(Configuration.GetSection("Redis:ConnString").Value);
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            services.AddMvc(options=>options.EnableEndpointRouting = false);
            services.AddRouting(options=> options.LowercaseUrls = true);
            var sqlVersion = new MySqlServerVersion(new Version(8, 0, 28));
            services.AddDbContext<SpotifyDbContexts>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("mysql"), sqlVersion);
            });
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("mysql"), sqlVersion);
            });

            services.AddIdentityCore<MusicLover>().AddEntityFrameworkStores<AuthDbContext>();
            services.AddScheduler();
            services.AddTransient<RefreshAppTokenHostedService>();
            services.AddTransient<ISpotifyAuth, SpotifyAuthHttpService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Audience = "apiusers";
                options.Authority = "iamcomputermann";
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration.GetSection("JWT:Issuer").ToString(),
                    ValidAudience = Configuration.GetSection("JWT:Audience").ToString(),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWT:Key").ToString()))
                };
            });
        }

        public void Configure(IApplicationBuilder app,IHostEnvironment env)
        {

            AddSchedulingServices(app);
            app.UseSerilogRequestLogging();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc();
            
        }


        private void AddSchedulingServices(IApplicationBuilder app)
        {
            var provider = app.ApplicationServices;
            provider.UseScheduler(schedulers =>
            {
                schedulers.Schedule<RefreshAppTokenHostedService>().Hourly();
                
            });
        }
    }
}
