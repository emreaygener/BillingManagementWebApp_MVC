using Newtonsoft.Json;

namespace BillingManagementWebApp.Middlewares
{
    public class CustomRequestHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomRequestHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Session.GetString("token");
            context.Session.SetString("originalRequest", context.Request.ToString());
            if (!string.IsNullOrEmpty(token))
            {
                context.Request.Headers.Add("Authorization", "Bearer " + token);
            }
            await _next(context);
        }
    }
    public static class CustomRequestHeaderMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomRequestHeaderMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomRequestHeaderMiddleware>();
        }
    }
}
