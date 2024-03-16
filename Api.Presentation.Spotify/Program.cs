
using Coravel;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using HashidsNet;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Spotify;
using Spotify.Configuration.SpotifyEndPoints;
using Spotify.Configuration;
using System.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace Api.Presentation.Spotify
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.AddServerHeader = false;
            });
            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
            try
            {
                builder.Host.UseSerilog();
                ConfigureServices(builder.Services, builder.Configuration);
                var app = builder.Build();
                Configure(app);
            }
            catch (Exception ex)
            {
                string type = ex.GetType().Name;
                if (!type.Equals("StopTheHostException", StringComparison.Ordinal))
                {
                    Log.Fatal(ex, "Something serious happened, Failed to start.Log better here");
                }

            }
            finally
            {
                Log.CloseAndFlush();
            }
            
        }
        private static void ConfigureServices(IServiceCollection services,IConfiguration configuration)
        {
           
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
            services.Configure<SpotifyAccessKey>(configuration.GetSection("Spotify"));
            services.Configure<JwtParams>(configuration.GetSection("JWT"));
            services.AddSingleton<IHashids>(new Hashids(configuration.GetSection("HashId:Salt").Value, 5));
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
        private static void Configure(WebApplication app)
        {
            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            //});

            //app.UseCoravelSchedulingServices();
            app.UseSerilogRequestLogging();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseAuthentication();
            //app.UseAuthorization();
            app.UseMvc();
            Log.Information("Application Starting Up:");
            app.Run();
        }
    }
}
