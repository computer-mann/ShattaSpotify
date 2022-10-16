using System.Globalization;

namespace Spotify.CustomMiddlewares
{
    public class SchedulingServiceMidddleware
    {
        private readonly RequestDelegate _next;

        public SchedulingServiceMidddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            

            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }
}
