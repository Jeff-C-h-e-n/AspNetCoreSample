using System.Text;

namespace CoinDesk.Handlers
{
    public class LoggingHttpMessageHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingHttpMessageHandler> _logger;

        public LoggingHttpMessageHandler(ILogger<LoggingHttpMessageHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var logContent = new StringBuilder();
            var requestBody = request.Content != null ? await request.Content.ReadAsStringAsync() : string.Empty;
            logContent.AppendLine($"Outgoing Request: {request.Method} {request.RequestUri}, Body: {requestBody}");

            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                var responseBody = response.Content != null ? await response.Content.ReadAsStringAsync() : string.Empty;
                logContent.AppendLine($"Incoming Response: {response.StatusCode}, Body: {responseBody}");
                _logger.LogInformation(logContent.ToString());
                return response;
            }
            catch (Exception ex)
            {
                logContent.AppendLine($"Error: {ex.Message}");
                _logger.LogError(ex, logContent.ToString());
                throw;
            }
        }
    }
}