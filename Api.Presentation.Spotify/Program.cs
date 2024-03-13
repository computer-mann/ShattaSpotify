
using Microsoft.AspNetCore.Builder;
using Serilog;
using Spotify;

namespace Api.Presentation.Spotify
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
            try
            {
                builder.Host.UseSerilog();
                var startup = new StartUp(builder.Configuration);
                startup.ConfigureServices(builder.Services, builder.WebHost);
                var app = builder.Build();
                startup.Configure(app, builder.Environment);
                Log.Information("Application Starting Up:");
                app.Run();
            }
            catch (Exception ex)
            {
                string type = ex.GetType().Name;
                if (!type.Equals("StopTheHostException", StringComparison.Ordinal))
                {
                    Log.Fatal(ex, "Something serious happened, Failed to start.");
                }

            }
            finally
            {
                Log.CloseAndFlush();
            }
            
        }
    }
}
