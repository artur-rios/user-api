using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Common.Dto;
using TechCraftsmen.User.Controllers;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Interfaces.Services;

namespace TechCraftsmen.User.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<TestController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<TestController> logger, IUserService user)
        {
            _logger = logger;
            _userService = user;
        }

        [HttpPost]
        [Route("/{id}")]
        public ResultDto<Core.Entities.User> GetUserById(int id)
        {
            try
            {
                var user = _userService.GetUserById(id);

                return new ResultDto<Core.Entities.User>(user, "User retrieved with success", true);
            }
            catch (NotFoundException nfe)
            {
                return new ResultDto<Core.Entities.User>(null, nfe.Message, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new ResultDto<Core.Entities.User>(null, "Internal error, please try again later", false);
            }
        }
    }
}
