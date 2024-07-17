using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Entities;
using TechCraftsmen.User.Core.Interfaces.Services;

namespace TechCraftsmen.User.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AuthenticationController(IAuthenticationService authService) : BaseController
    {
        private readonly IAuthenticationService _authService = authService;

        [HttpPost]
        [Route("User")]
        [AllowAnonymous]
        public ActionResult<ResultDto<AuthenticationToken>> AuthenticateUser(AuthenticationCredentialsDto credentialsDto)
        {
            var authToken = _authService.AuthenticateUser(credentialsDto);

            return Success(authToken, "User authenticated with success");
        }
    }
}
