using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.ValueObjects;
using TechCraftsmen.User.Services.Dto;
using TechCraftsmen.User.Services.Interfaces;
using TechCraftsmen.User.WebApi.ValueObjects;
using Results = TechCraftsmen.User.Core.Enums.Results;

namespace TechCraftsmen.User.WebApi.Authorization
{
    public class JwtMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context, IAuthenticationService authService)
        {
            Endpoint? endpoint = context.GetEndpoint();
            
            if (endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() is null)
            {
                bool authenticated = ValidateTokenAndAuthenticate(context, authService, out string[] messages);

                if (!authenticated)
                {
                    await ResolveUnauthorized(context, messages);

                    return;
                }
            }
            
            await next(context);
        }

        private static bool ValidateTokenAndAuthenticate(HttpContext context, IAuthenticationService authService, out string[] messages)
        {
            string? token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

            ServiceOutput<bool> validationResult =
                authService.ValidateJwtToken(token!, out UserDto? authenticatedUser);
            
            messages = validationResult.Messages;

            if (validationResult.Result is not Results.Success || !validationResult.Data)
            {
                return false;
            }

            if (!RoleIsAllowedOnEnvironment(authenticatedUser!.RoleId))
            {
                messages = ["Test user can't be used outside of test environment"];
                
                return false;
            }

            context.Items["User"] = authenticatedUser;

            return true;
        }

        private static bool RoleIsAllowedOnEnvironment(int roleId)
        {
            if (roleId != (int)Roles.Test)
            {
                return true;
            }

            string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return environment!.Equals("Local");
        }

        private static async Task ResolveUnauthorized(HttpContext context, string[] messages)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            
            WebApiOutput<string> response = new(string.Empty, messages);
            
            string payload = JsonConvert.SerializeObject(response);

            await context.Response.WriteAsync(payload);
        }
    }
}
