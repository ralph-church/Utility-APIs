using Microsoft.AspNetCore.Builder;
using repair.service.shared.exception.middleware;
using repair.service.shared.exception.model;

namespace repair.service.shared.exception.extensions
{
    /// <summary>
    /// Extension class of IApplicationBuilder class and used register the ExceptionMiddleware type.
    /// </summary>
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app, ExceptionOption option)
        {
            app.UseMiddleware<ExceptionMiddleware>(option);
        }
            
    }
}
