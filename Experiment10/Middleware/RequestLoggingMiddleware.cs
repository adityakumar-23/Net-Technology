using System.Diagnostics;

namespace StudentManagementAPI.Middleware
{
    /// <summary>
    /// Custom request logging middleware that records HTTP method,
    /// request path, response status code, and elapsed execution time.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var method = context.Request.Method;
            var path = context.Request.Path;
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            _logger.LogInformation("--> Incoming Request: [{Method}] {Path} from {IP}", method, path, ipAddress);

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var statusCode = context.Response.StatusCode;

                _logger.LogInformation("<-- Outgoing Response: [{Method}] {Path} responded {StatusCode} in {Elapsed}ms",
                    method, path, statusCode, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
