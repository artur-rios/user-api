using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Interfaces.Services;

namespace TechCraftsmen.User.Api.Controllers
{
    [ApiController]
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
        [Route("/{id}")]
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
        [Route("/Filter")]
        public ActionResult<ResultDto<IEnumerable<UserDto>>> GetUsersByFilter()
        {
            try
            {
                var users = _userService.GetUsersByFilter(HttpContext.Request.Query);

                return Success(users, "Search completed with success");
            }
            catch (NotAllowedException nae)
            {
                return BadRequest<IEnumerable<UserDto>>(nae);
            }
            catch (NotFoundException nfe)
            {
                return NotFound<IEnumerable<UserDto>>(nfe);
            }
            catch (Exception ex)
            {
                return Error<IEnumerable<UserDto>>(ex);
            }
        }

        [HttpPut]
        [Route("/Update")]
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
        [Route("/{id}/Activate")]
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
        [Route("/{id}/Deactivate")]
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
        [Route("/{id}/Delete")]
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
