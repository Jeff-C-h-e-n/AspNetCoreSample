using System.Text;

namespace CoinDesk.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var logContent = new StringBuilder();

            try
            {
                context.Request.EnableBuffering();
                using var requestBodyReader = new StreamReader(context.Request.Body);
                var requestBody = await requestBodyReader.ReadToEndAsync();
                context.Request.Body.Position = 0;
                logContent.AppendLine($"Incoming Request: {context.Request.Method} {context.Request.Path}, Body: {requestBody}");

                var originalResponseBodyStream = context.Response.Body;
                using (var responseBodyStream = new MemoryStream())
                {
                    context.Response.Body = responseBodyStream;
                    await _next(context);
                    context.Response.Body.Position = 0;
                    using var responseBodyReader = new StreamReader(context.Response.Body);
                    var responseBody = await responseBodyReader.ReadToEndAsync();
                    context.Response.Body.Position = 0;
                    responseBodyStream.Position = 0;
                    await responseBodyStream.CopyToAsync(originalResponseBodyStream);
                    logContent.AppendLine($"Outgoing Response: {context.Response.StatusCode}, Body: {responseBody}");
                }
            }
            finally
            {
                _logger.LogInformation(logContent.ToString());
            }
        }
    }
}