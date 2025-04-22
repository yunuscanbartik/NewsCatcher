using System.Net;
using System.Text.Json;
using NewsCatcher.Models.Models;

namespace NewsCatcherApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
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



