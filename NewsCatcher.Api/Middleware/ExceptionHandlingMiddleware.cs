using System.Net;
using System.Text.Json;
using NewsCatcher.Models.Models;
using Pinqloq;

namespace NewsCatcherApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IPinqloqLogger _pinqloqLogger;

        public ExceptionHandlingMiddleware(
            RequestDelegate requestDelegate,
            ILogger<ExceptionHandlingMiddleware> logger,
            IPinqloqLogger pinqloqLogger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
            _pinqloqLogger = pinqloqLogger;
        }

        public async Task InvokeAsync(HttpContext httpcontext)
        {
            try
            {
                await _requestDelegate(httpcontext);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unhandled exception. TraceId: {TraceId}", httpcontext.TraceIdentifier);
                _pinqloqLogger.Enqueue(new PinqloqLogEntry
                {
                    Event = $"unhandled_exception: {exception.GetType().Name}",
                    LogLevel = PinqloqLogLevel.Error,
                    LogSourceType = PinqloqLogSourceType.Backend,
                    Metadata = new Dictionary<string, string>
                    {
                        ["traceId"] = httpcontext.TraceIdentifier,
                        ["method"] = httpcontext.Request.Method,
                        ["path"] = httpcontext.Request.Path
                    },
                    Detail = new Dictionary<string, string>
                    {
                        ["message"] = exception.Message,
                        ["stack"] = exception.StackTrace ?? string.Empty
                    }
                });
                await WriteErrorResponseAsync(httpcontext, exception);
            }
        }

        private static async Task WriteErrorResponseAsync(HttpContext httpcontext, Exception exexception)
        {
            if (httpcontext.Response.HasStarted)
            {
                return;
            }

            httpcontext.Response.Clear();
            httpcontext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpcontext.Response.ContentType = "application/json";

            var payload = new ReturnModel
            {
                Status = false,
                Message = "Unexpected error occurred.",
                ErrorCode = "UNHANDLED_EXCEPTION",
                ErrorMessage = exexception.Message,
                RequestId = httpcontext.TraceIdentifier,
                StatusCode = httpcontext.Response.StatusCode,
                RequestTime = DateTime.Now,
                ResponseTime = DateTime.Now
            };

            var json = JsonSerializer.Serialize(payload);
            await httpcontext.Response.WriteAsync(json);
        }
    }
}



