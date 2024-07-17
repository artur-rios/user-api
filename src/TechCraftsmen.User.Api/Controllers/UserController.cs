using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Configuration.Authorization;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;
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
        [RoleRequirement(Roles.ADMIN)]
        public ActionResult<ResultDto<int>> CreateUser(UserDto userDto)
        {
            var userId = _userService.CreateUser(userDto);

            return Created(userId, "User created with success");
        }

        [HttpGet]
        [Route("{id}")]
        [RoleRequirement(Roles.ADMIN, Roles.TEST)]
        public ActionResult<ResultDto<UserDto>> GetUserById(int id)
        {
            var user = _userService.GetUserById(id);

            return Success(user, "User retrieved with success");
        }

        [HttpGet]
        [Route("Filter")]
        [RoleRequirement(Roles.ADMIN)]
        public ActionResult<ResultDto<IList<UserDto>>> GetUsersByFilter()
        {
            var users = _userService.GetUsersByFilter(HttpContext.Request.Query);

            return Success(users, "Search completed with success");
        }

        [HttpPut]
        [Route("Update")]
        [RoleRequirement(Roles.ADMIN)]
        public ActionResult<ResultDto<UserDto>> UpdateUser(UserDto userDto)
        {
            _userService.UpdateUser(userDto);

            return NoContent<UserDto>("User updated with success");
        }

        [HttpPatch]
        [Route("{id}/Activate")]
        [RoleRequirement(Roles.ADMIN)]
        public ActionResult<ResultDto<UserDto>> ActivateUser(int id)
        {
            _userService.ActivateUser(id);

            return NoContent<UserDto>("User activated with success");
        }

        [HttpPatch]
        [Route("{id}/Deactivate")]
        [RoleRequirement(Roles.ADMIN)]
        public ActionResult<ResultDto<UserDto>> DeactivateUser(int id)
        {
            _userService.DeactivateUser(id);

            return NoContent<UserDto>("User deactivated with success");
        }

        [HttpDelete]
        [Route("{id}/Delete")]
        [RoleRequirement(Roles.ADMIN)]
        public ActionResult<ResultDto<UserDto>> DeleteUser(int id)
        {
            _userService.DeleteUser(id);

            return NoContent<UserDto>("User deleted with success");
        }
    }
}
