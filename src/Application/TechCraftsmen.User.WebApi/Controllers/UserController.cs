using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.WebApi.Authorization;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Filters;
using TechCraftsmen.User.Core.ValueObjects;
using TechCraftsmen.User.Services.Dto;
using TechCraftsmen.User.Services.Interfaces;
using TechCraftsmen.User.WebApi.ValueObjects;

namespace TechCraftsmen.User.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController(IUserService user) : BaseController
    {
        [HttpPost]
        [Route("Create")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<WebApiOutput<int>> CreateUser([FromBody] UserDto userDto)
        {
            ServiceOutput<int> userId = user.CreateUser(userDto);

            return Resolve(userId);
        }

        [HttpGet]
        [Route("Filter")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<WebApiOutput<IList<UserDto>>> GetUsersByFilter([FromQuery] UserFilter filter)
        {
            ServiceOutput<IList<UserDto>> users = user.GetUsersByFilter(filter);

            return Resolve(users);
        }

        [HttpPut]
        [Route("Update")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<WebApiOutput<UserDto>> UpdateUser([FromBody] UserDto userDto)
        {
            ServiceOutput<UserDto?> updatedUser = user.UpdateUser(userDto);

            return Resolve(updatedUser)!;
        }

        [HttpPatch]
        [Route("{id:int}/Activate")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<WebApiOutput<int>> ActivateUser([FromRoute] int id)
        {
            ServiceOutput<int> activatedUserId = user.ActivateUser(id);

            return Resolve(activatedUserId);
        }

        [HttpPatch]
        [Route("{id:int}/Deactivate")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<WebApiOutput<int>> DeactivateUser([FromRoute] int id)
        {
            ServiceOutput<int> deactivatedUserId = user.DeactivateUser(id);

            return Resolve(deactivatedUserId);
        }

        [HttpDelete]
        [Route("{id:int}/Delete")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<WebApiOutput<int>> DeleteUser([FromRoute] int id)
        {
            ServiceOutput<int> deletedUserId = user.DeleteUser(id);

            return Resolve(deletedUserId);
        }
    }
}
