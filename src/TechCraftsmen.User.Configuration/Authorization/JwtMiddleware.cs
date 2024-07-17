using Microsoft.AspNetCore.Http;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Interfaces.Services;

namespace TechCraftsmen.User.Configuration.Authorization
{
    public class JwtMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context, IAuthenticationService authService)
        {
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

            if (authService.ValidateJwtToken(token!, out var authenticatedUser))
            {
                if (authenticatedUser!.RoleId == (int)Roles.TEST)
                {
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    if (!environment!.Equals("Local"))
                    {
                        throw new NotAllowedException("Test user can't be used outside of test environment");
                    }
                }

                context.Items["User"] = authenticatedUser;
            }

            await _next(context);
        }
    }
}
