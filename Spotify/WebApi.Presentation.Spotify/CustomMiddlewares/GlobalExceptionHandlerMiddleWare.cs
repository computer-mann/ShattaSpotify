using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Presentation.Spotify.CustomMiddlewares
{
    internal class GlobalExceptionHandlerMiddleWare : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleWare> _logger;

        public GlobalExceptionHandlerMiddleWare(ILogger<GlobalExceptionHandlerMiddleWare> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogCritical(exception,"Serious Unexpected fault: {message}",exception.Message);

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails()
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Internal Server Error",
                Detail = "Something happened on our end. Try again",
            });
            return true;
        }
    }
}
