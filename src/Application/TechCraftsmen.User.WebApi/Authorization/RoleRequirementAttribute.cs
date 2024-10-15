using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Core.Enums;

namespace TechCraftsmen.User.WebApi.Authorization
{
    public class RoleRequirementAttribute : TypeFilterAttribute
    {
        public RoleRequirementAttribute(params Roles[] authorizedRoles) : base(typeof(RoleRequirementFilter))
        {
            object[] arguments = [authorizedRoles];

            Arguments = arguments;
        }
    }
}
