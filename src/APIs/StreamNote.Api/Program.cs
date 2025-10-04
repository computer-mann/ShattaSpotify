using Serilog;
using StreamNote.Api.CustomMiddlewares;
using StreamNote.Api.HostedServices;
using Microsoft.OpenApi.Models;
using StreamNote.Database.Commons.Options;
using StreamNote.Database.Commons.Configuration;

namespace StreamNote.Api
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
            services.AddExceptionHandler<GlobalExceptionHandlerMiddleWare>();
            services.AddProblemDetails();
            services.AddTransient<WeeklyDbSongClearanceHostedService>();
            services.AddTransient<RefreshAppTokenCoravelService>();
            services.AddTransient<CheckNewReleasesHostedService>();
            services.AddTransient<GoogleNotificationHostedService>();
            services.AddTransient<RefreshUserTokenHostedService>();
            
            //FirebaseApp.Create(new AppOptions()
            //{
            //    Credential = GoogleCredential.FromFile("D:\\Projects\\Projects\\cmfirstapp-1941d-firebase-adminsdk-60ady-1258ed7d20.json"),
            //    ProjectId = "cmfirstapp-1941d"
            //});

            services.AddControllers();
            // services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StreamNote", Version = "v1" }); 
            });

           // services.AddScheduler();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddHttpClient();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddOptions<SpotifyAccessConfig>().Bind(configuration.GetSection("SpotifyAccessConfig")).ValidateDataAnnotations().ValidateOnStart();
            services.AddOptions<JwtParamOptions>().Bind(configuration.GetSection("JwtParamOptions")).ValidateDataAnnotations().ValidateOnStart();
            services.AddKafkaProducer();
            services.AddRedisOm(configuration);
              //services.AddDbContext<AuthDbContext>(options =>
              //{
                  
              //});

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
            //services.AddRateLimiter();
        }
        private static void Configure(WebApplication app)
        {
            app.UseStatusCodePages();
            app.UseExceptionHandler();
            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            //});

            app.UseCoravelSchedulingServices();

            //app.UseSerilogRequestLogging();
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
