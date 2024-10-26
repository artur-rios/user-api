using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TechCraftsmen.User.Domain.Enums;
using TechCraftsmen.User.Services.Dto;

namespace TechCraftsmen.User.WebApi.Authorization
{
    public class RoleRequirementFilter(params Roles[] authorizedRoles) : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            UserDto? user = context.HttpContext.Items["User"] as UserDto;

            bool authorized = false;

            if (user is not null)
            {
                authorized = authorizedRoles.Any(role => role == (Roles)user.RoleId);
            }

            if (!authorized)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
