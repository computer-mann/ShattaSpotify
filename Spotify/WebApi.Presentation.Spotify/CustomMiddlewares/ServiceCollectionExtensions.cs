using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spotify.Services;

namespace Api.Presentation.Spotify.CustomMiddlewares
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddTypedHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<SpotifyHttpService>();

        }
    }
}
