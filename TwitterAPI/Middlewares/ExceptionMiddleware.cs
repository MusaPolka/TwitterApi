using System.Net;
using TwitterAPI.Exceptions;

namespace TwitterAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsJsonAsync(new ExceptionDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            });
        }
    }
}
