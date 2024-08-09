using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Configuration.Authorization;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Filters;
using TechCraftsmen.User.Core.Interfaces.Services;

namespace TechCraftsmen.User.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController(IUserService user) : BaseController
    {
        private readonly IUserService _userService = user;

        [HttpPost]
        [Route("Create")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<DataResultDto<int>> CreateUser([FromBody] UserDto userDto)
        {
            OperationResultDto<int> userId = _userService.CreateUser(userDto);

            return Resolve(userId);
        }

        [HttpGet]
        [Route("Filter")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<DataResultDto<IList<UserDto>>> GetUsersByFilter([FromQuery] UserFilter filter)
        {
            OperationResultDto<IList<UserDto>> users = _userService.GetUsersByFilter(filter);

            return Resolve(users);
        }

        [HttpPut]
        [Route("Update")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<DataResultDto<UserDto>> UpdateUser([FromBody] UserDto userDto)
        {
            OperationResultDto<UserDto?> updatedUser = _userService.UpdateUser(userDto);

            return Resolve(updatedUser)!;
        }

        [HttpPatch]
        [Route("{id:int}/Activate")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<DataResultDto<int>> ActivateUser([FromRoute] int id)
        {
            OperationResultDto<int> activatedUserId = _userService.ActivateUser(id);

            return Resolve(activatedUserId);
        }

        [HttpPatch]
        [Route("{id:int}/Deactivate")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<DataResultDto<int>> DeactivateUser([FromRoute] int id)
        {
            OperationResultDto<int> deactivatedUserId = _userService.DeactivateUser(id);

            return Resolve(deactivatedUserId);
        }

        [HttpDelete]
        [Route("{id:int}/Delete")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<DataResultDto<int>> DeleteUser([FromRoute] int id)
        {
            OperationResultDto<int> deletedUserId = _userService.DeleteUser(id);

            return Resolve(deletedUserId);
        }
    }
}
