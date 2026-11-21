using System.Net;
using System.Text.Json;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Middleware
{
    /// <summary>
    /// Custom exception handling middleware to catch unhandled exceptions,
    /// log error details, and return standardized JSON error responses.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred during request: {Method} {Path}",
                    context.Request.Method, context.Request.Path);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorResponse = new ApiErrorResponse
            {
                StatusCode = context.Response.StatusCode,
                Message = "An unexpected error occurred while processing your request. Please try again later.",
                Details = exception.Message,
                Timestamp = DateTime.UtcNow
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(errorResponse, options);

            return context.Response.WriteAsync(json);
        }
    }
}
