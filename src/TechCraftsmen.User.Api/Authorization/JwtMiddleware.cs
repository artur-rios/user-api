using TechCraftsmen.User.Core.Interfaces.Services;

namespace TechCraftsmen.User.Api.Authorization
{
    public class JwtMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context, IAuthenticationService authService, IUserService userService)
        {
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

            if (authService.ValidateJwtToken(token!, out var authenticatedUser))
            {
                context.Items["User"] = authenticatedUser;
            }

            await _next(context);
        }
    }
}
