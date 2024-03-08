using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Common.Dto;
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
                return NotFound(nfe.Message);
            }
            catch (Exception ex)
            {
                return Error<UserDto>(ex);
            }
        }
    }
}
