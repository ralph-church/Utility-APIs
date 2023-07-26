using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using repair.service.shared.exception.infrastructure;
using repair.service.shared.exception.model;
using System;
using System.Threading.Tasks;

namespace repair.service.shared.exception.middleware
{
    /// <summary>
    /// Custom class that used to handle the unhandled exceptions.
    /// </summary>
    public class ExceptionMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly ExceptionOption _option;

        public ExceptionMiddleware(RequestDelegate next, ExceptionOption option, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _next = next;
            _option = option;
        }
        public virtual async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (System.Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
                _logger.LogError(ex.Message);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, System.Exception exception)
        {
            context.Response.ContentType = "application/json";

            var errorModel = ExceptionFactory.GetErrorModel(exception, _option);

            context.Response.StatusCode = errorModel.StatusCode;
            _logger.LogError(errorModel.ToString());
            return context.Response.WriteAsync(errorModel.ToString());
        }
    }
}
