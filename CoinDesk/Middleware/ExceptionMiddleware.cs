using CoinDesk.Resources;
using Microsoft.Extensions.Localization;
using System.Net;

namespace CoinDesk.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionMiddleware> _logger;

        private readonly IStringLocalizer<SharedResource> _localizer;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IStringLocalizer<SharedResource> localizer)
        {
            _next = next;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await context.Response.WriteAsJsonAsync(new { Message = _localizer["Internal Server Error. Please try again later."].Value });
            }
        }
    }
}