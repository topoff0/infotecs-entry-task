using System.Net;
using System.Text.Json;
using Application.Core.Exceptions;

namespace Application.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomValidationException ex)
            {
                await WriteErrorResponse(context,
                                         HttpStatusCode.BadRequest,
                                         "Validation error",
                                         ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An unexpected error occurred while executing request: {Path}", context.Request.Path);
                await WriteErrorResponse(context,
                            HttpStatusCode.InternalServerError,
                            "Internal server error");
            }
        }

        private async Task WriteErrorResponse(HttpContext context,
                                              HttpStatusCode statusCode,
                                              string error,
                                              string? details = null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                error,
                details,
                statusCode = (int)statusCode,
                timestamp = DateTime.UtcNow
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}