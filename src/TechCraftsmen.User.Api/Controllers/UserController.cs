using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Api.Authorization;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Interfaces.Services;

namespace TechCraftsmen.User.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : BaseController
    {

        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService user) : base(logger)
        {
            _userService = user;
        }

        [HttpPost]
        [Route("Create")]
        [AllowAnonymous]
        //[RoleRequirement(Roles.ADMIN)]
        public ActionResult<ResultDto<int>> CreateUser(UserDto userDto)
        {
            try
            {
                var userId = _userService.CreateUser(userDto);

                return Created(userId, "User created with success");
            }
            catch (ValidationException ve)
            {
                return BadRequest<int>(ve);
            }
            catch (Exception ex)
            {
                return Error<int>(ex);
            }
        }

        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        //[RoleRequirement(Roles.ADMIN)]
        public ActionResult<ResultDto<UserDto>> GetUserById(int id)
        {
            try
            {
                var user = _userService.GetUserById(id);

                return Success(user, "User retrieved with success");
            }
            catch (NotFoundException nfe)
            {
                return NotFound<UserDto>(nfe);
            }
            catch (Exception ex)
            {
                return Error<UserDto>(ex);
            }
        }

        [HttpGet]
        [Route("Filter")]
        [RoleRequirement(Roles.ADMIN)]
        public ActionResult<ResultDto<IList<UserDto>>> GetUsersByFilter()
        {
            try
            {
                var users = _userService.GetUsersByFilter(HttpContext.Request.Query);

                return Success(users, "Search completed with success");
            }
            catch (NotAllowedException nae)
            {
                return BadRequest<IList<UserDto>>(nae);
            }
            catch (NotFoundException nfe)
            {
                return NotFound<IList<UserDto>>(nfe);
            }
            catch (Exception ex)
            {
                return Error<IList<UserDto>>(ex);
            }
        }

        [HttpPut]
        [Route("Update")]
        [RoleRequirement(Roles.ADMIN)]
        public ActionResult<ResultDto<UserDto>> UpdateUser(UserDto userDto)
        {
            try
            {
                _userService.UpdateUser(userDto);

                return NoContent<UserDto>("User updated with success");
            }
            catch (NotAllowedException nae)
            {
                return BadRequest<UserDto>(nae);
            }
            catch (NotFoundException nfe)
            {
                return NotFound<UserDto>(nfe);
            }
            catch (Exception ex)
            {
                return Error<UserDto>(ex);
            }
        }

        [HttpPatch]
        [Route("{id}/Activate")]
        [RoleRequirement(Roles.ADMIN)]
        public ActionResult<ResultDto<UserDto>> ActivateUser(int id)
        {
            try
            {
                _userService.ActivateUser(id);

                return NoContent<UserDto>("User activated with success");
            }
            catch (NotAllowedException nae)
            {
                return BadRequest<UserDto>(nae);
            }
            catch (NotFoundException nfe)
            {
                return NotFound<UserDto>(nfe);
            }
            catch (Exception ex)
            {
                return Error<UserDto>(ex);
            }
        }

        [HttpPatch]
        [Route("{id}/Deactivate")]
        [RoleRequirement(Roles.ADMIN)]
        public ActionResult<ResultDto<UserDto>> DeactivateUser(int id)
        {
            try
            {
                _userService.DeactivateUser(id);

                return NoContent<UserDto>("User deactivated with success");
            }
            catch (NotAllowedException nae)
            {
                return BadRequest<UserDto>(nae);
            }
            catch (NotFoundException nfe)
            {
                return NotFound<UserDto>(nfe);
            }
            catch (Exception ex)
            {
                return Error<UserDto>(ex);
            }
        }

        [HttpDelete]
        [Route("{id}/Delete")]
        [RoleRequirement(Roles.ADMIN)]
        public ActionResult<ResultDto<UserDto>> DeleteUser(int id)
        {
            try
            {
                _userService.DeleteUser(id);

                return NoContent<UserDto>("User deleted with success");
            }
            catch (NotAllowedException nae)
            {
                return BadRequest<UserDto>(nae);
            }
            catch (NotFoundException nfe)
            {
                return NotFound<UserDto>(nfe);
            }
            catch (Exception ex)
            {
                return Error<UserDto>(ex);
            }
        }
    }
}
