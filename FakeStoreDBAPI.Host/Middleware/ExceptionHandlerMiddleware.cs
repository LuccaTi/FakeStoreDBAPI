using FakeStoreDBAPI.Host.Exceptions;
using System.Net;
using System.Text.Json;

namespace FakeStoreDBAPI.Host.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
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

                var statusCode = HttpStatusCode.InternalServerError;
                var message = "An internal server error has occured.";

                if (ex is NotFoundException notFoundException)
                {
                    statusCode = HttpStatusCode.NotFound;
                    message = notFoundException.Message;
                    _logger.LogWarning($"Resource not found: {notFoundException.Message}");
                }
                else if (ex is InvalidIdException invalidIdException)
                {
                    statusCode = HttpStatusCode.BadRequest;
                    message = invalidIdException.Message;
                    _logger.LogWarning($"Invalid ID provided: {invalidIdException.Message}");
                }
                else if (ex is InvalidResourceException invalidResourceException)
                {
                    statusCode = HttpStatusCode.BadRequest;
                    message = invalidResourceException.Message;
                    _logger.LogWarning($"Invalid resource: {invalidResourceException.Message}");
                }
                else if (ex is ConflictException conflictException)
                {
                    statusCode = HttpStatusCode.Conflict;
                    message = conflictException.Message;
                    _logger.LogWarning($"Conflict occurred: {conflictException.Message}");
                }
                else
                {
                    _logger.LogError(ex, $"An unhandled exception has occurred: {ex.Message}");
                }

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = message
                };

                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
