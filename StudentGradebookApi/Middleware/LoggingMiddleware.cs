using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace StudentGradebookApi.Middleware

{
    public class LoggingMiddleware
    {
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public LoggingMiddleware(ILogger<LoggingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Guid correlationId = Guid.NewGuid();
            var stopwatch = Stopwatch.StartNew();
            try
            {
                
                _logger.LogInformation("---------------------------");

                _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path} from {context.Connection.RemoteIpAddress?.ToString()} User-Agent: {context.Request.Headers.UserAgent.ToString()} CorrelationId: {correlationId.ToString()}");

                await _next(context); //Call next middleware
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex}, CorrelationId: {correlationId}");
                HandleException(ex, context);
            }
            finally
            {
                stopwatch.Stop();

                _logger.LogInformation($"Response: {context.Request.Method} {context.Request.Path} Status: {context.Response.StatusCode} Duration: {stopwatch.ElapsedMilliseconds}ms CorrelationId: {correlationId.ToString()}");

                _logger.LogInformation("---------------------------");
            }
        }

        private Task HandleException(Exception exception, HttpContext context) 
        { 
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
