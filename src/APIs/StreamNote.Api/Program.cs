using Coravel;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Serilog;
using StreamNote.Api.CustomMiddlewares;
using StreamNote.Api.HostedServices;
using StreamNote.Database.Commons.Configuration;
using StreamNote.Database.Commons.Options;

namespace StreamNote.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            var configuration = builder.Configuration;
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.AddServerHeader = false;
            });

            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

            try
            {
                
                builder.Host.UseSerilog();

                services.AddControllers();
                services.AddEndpointsApiExplorer();

                services.AddExceptionHandler<GlobalExceptionHandlerMiddleWare>();
                services.AddProblemDetails();
                services.AddTransient<WeeklyDbSongClearanceHostedService>();
                services.AddTransient<RefreshAppTokenCoravelService>();
                services.AddTransient<CheckNewReleasesHostedService>();
                services.AddTransient<GoogleNotificationHostedService>();
                services.AddTransient<RefreshUserTokenHostedService>();

                
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StreamNote", Version = "v1" });
                });

                 services.AddScheduler();
                  services.AddRouting(options => options.LowercaseUrls = true);
                services.AddHttpClient();

                services.AddOptions<SpotifyAccessConfig>().Bind(configuration.GetSection("SpotifyAccessConfig")).ValidateDataAnnotations().ValidateOnStart();
                services.AddOptions<JwtParamOptions>().Bind(configuration.GetSection("JwtParamOptions")).ValidateDataAnnotations().ValidateOnStart();
                services.AddKafkaProducer();
               
                services.AddRedisOm(configuration);
                //services.AddIdentityCore<MusicNerd>().AddEntityFrameworkStores<AuthDbContext>();


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

                var app = builder.Build();
                app.UseCoravelSchedulingServices();

                // app.UseSerilogRequestLogging();
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseAuthentication();
                app.UseAuthorization();
                Log.Information("Application Starting Up:");
                app.MapControllers();
                app.Run();
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
        
    }
}
