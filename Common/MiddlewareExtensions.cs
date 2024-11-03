using Common.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Common {
    public static class MiddlewareExtensions {
        public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder) {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
