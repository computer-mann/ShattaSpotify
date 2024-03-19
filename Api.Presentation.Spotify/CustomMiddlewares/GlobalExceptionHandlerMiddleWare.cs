using Google.Apis.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
