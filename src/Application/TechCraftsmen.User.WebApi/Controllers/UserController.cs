using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.WebApi.Authorization;
using TechCraftsmen.User.Domain.Enums;
using TechCraftsmen.User.Services.Dto;
using TechCraftsmen.User.Services.Filters;
using TechCraftsmen.User.Services.Interfaces;
using TechCraftsmen.User.Services.Output;

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
        public ActionResult<Output<int>> CreateUser([FromBody] UserDto userDto)
        {
            ServiceOutput<int> userId = user.CreateUser(userDto);

            return Resolve(userId);
        }

        [HttpGet]
        [Route("Filter")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<Output<IList<UserDto>>> GetUsersByFilter([FromQuery] UserFilter filter)
        {
            ServiceOutput<IList<UserDto>> users = user.GetUsersByFilter(filter);

            return Resolve(users);
        }

        [HttpPut]
        [Route("Update")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<Output<UserDto>> UpdateUser([FromBody] UserDto userDto)
        {
            ServiceOutput<UserDto?> updatedUser = user.UpdateUser(userDto);

            return Resolve(updatedUser)!;
        }

        [HttpPatch]
        [Route("{id:int}/Activate")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<Output<int>> ActivateUser([FromRoute] int id)
        {
            ServiceOutput<int> activatedUserId = user.ActivateUser(id);

            return Resolve(activatedUserId);
        }

        [HttpPatch]
        [Route("{id:int}/Deactivate")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<Output<int>> DeactivateUser([FromRoute] int id)
        {
            ServiceOutput<int> deactivatedUserId = user.DeactivateUser(id);

            return Resolve(deactivatedUserId);
        }

        [HttpDelete]
        [Route("{id:int}/Delete")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<Output<int>> DeleteUser([FromRoute] int id)
        {
            ServiceOutput<int> deletedUserId = user.DeleteUser(id);

            return Resolve(deletedUserId);
        }
    }
}
