using Microsoft.EntityFrameworkCore;
using Serilog;
using Spotify.Configuration;
using Spotify.Configuration.SpotifyEndPoints;
using Spotify.Models;
using StackExchange.Redis;
using Spotify.Areas.Auth.Models.DbContext;
using Spotify.Areas.Auth.Models;
using Coravel;
using Spotify.Services.Interfaces;
using Spotify.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.HttpOverrides;
using Spotify.CustomMiddlewares;
using Spotify.Utilities;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using HashidsNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

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
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("C:\\Users\\hpsn1\\OneDrive\\Documents\\Projects\\cmfirstapp-1941d-firebase-adminsdk-60ady-1258ed7d20.json"),
                ProjectId = "cmfirstapp-1941d"
            });

            services.AddControllers();
           // services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
           
            services.AddScheduler();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddHttpClient();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.Configure<SpotifyAccessKey>(Configuration.GetSection("Spotify"));
            services.Configure<JwtParams>(Configuration.GetSection("JWT"));
            services.AddSingleton<IHashids>(new Hashids(Configuration.GetSection("HashId:Salt").Value, 5));
            /*  var multiplexer = ConnectionMultiplexer.Connect(Configuration.GetSection("Redis:ConnString").Value);
              services.AddSingleton<IConnectionMultiplexer>(multiplexer);
               services.AddTransient<RefreshAppTokenCoravelService>();
             services.AddTransient<ISpotifyAuth, SpotifyAuthHttpService>();

              var sqlVersion = new MySqlServerVersion(new Version(8, 0, 28));
              string connectionString = Configuration.GetConnectionString("mysql");
              services.AddDbContext<SpotifyDbContexts>(options =>
              {

                  options.UseMySql(connectionString, sqlVersion);
              });
              services.AddDbContext<AuthDbContext>(options =>
              {
                  options.UseMySql(connectionString, sqlVersion);
              });*/

            // services.AddIdentityCore<MusicNerd>().AddEntityFrameworkStores<AuthDbContext>();


            //services.AddTransient<IAuthUtils,AuthUtils>();
            /* services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
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
             });*/
        }

        public void Configure(IApplicationBuilder app,IHostEnvironment env)
        {
            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            //});

            //app.UseCoravelSchedulingServices();
            app.UseSerilogRequestLogging();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseAuthentication();
            //app.UseAuthorization();
            app.UseMvc();
            
            
        }


        
    }
}
