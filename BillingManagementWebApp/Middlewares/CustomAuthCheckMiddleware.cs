using BillingManagementWebApp.Auth.Model;
using Microsoft.AspNetCore.Authentication;

namespace BillingManagementWebApp.Middlewares
{
    public class CustomAuthCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpClient _httpClient;

        public CustomAuthCheckMiddleware(RequestDelegate next)
        {
            _next = next;
            _httpClient = new HttpClient();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated == false && context.Request.Headers.Authorization.Any())
            {
                var refreshToken = context.Session.GetString("token");
                if(_httpClient.DefaultRequestHeaders.Authorization is not null)
                {
                    await _httpClient.GetAsync("https://localhost:7047/Login/refreshToken");
                    var newToken = context.Session.GetString("token");
                    context.Request.Headers.Add("Authorization", "Bearer " + newToken);
                    await _next(context);
                }

            }
            await _next(context);
        }
    }
    public static class CustomAuthCheckMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomAuthCheckMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomAuthCheckMiddleware>();
        }
    }
}
