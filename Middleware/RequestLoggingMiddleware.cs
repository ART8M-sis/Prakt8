using System.Diagnostics;
using FileAvatar.Logging;

namespace FileAvatar.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IFileRequestLogger fileLogger)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();

                var method = context.Request.Method;
                var path = context.Request.Path;
                var statusCode = context.Response.StatusCode;
                var durationMs = stopwatch.ElapsedMilliseconds;

                var message = $"{method} {path} => {statusCode} executed in {durationMs}ms";

                _logger.LogInformation(
                    "Request {method} {url} => {statusCode} executed in {elapsedMilliseconds}ms",
                    method, path, statusCode, durationMs);

                await fileLogger.LogAsync(new RequestLogEntry
                {
                    Timestamp = DateTime.UtcNow,
                    Method = method,
                    Path = path,
                    StatusCode = statusCode,
                    DurationMs = durationMs,
                    Message = message
                });
            }
        }
    }

    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}