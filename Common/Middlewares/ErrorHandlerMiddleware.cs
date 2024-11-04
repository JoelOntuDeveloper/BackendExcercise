using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Common.Middlewares {
    public class ErrorHandlerMiddleware {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger) {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context) {
            try {
                await _next(context);
            } catch (NotFoundException ex) {
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
            } catch (ValidationException ex) {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            } catch (FormatException ex) {
                await HandleExceptionAsync(context, ex, HttpStatusCode.Conflict);
            } catch (Exception ex) {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode) {
            _logger.LogError(exception, exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new { error = exception.Message };
            var options = new JsonSerializerOptions {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            var result = JsonSerializer.Serialize(errorResponse, options);
            return context.Response.WriteAsync(result);
        }

    }
}
