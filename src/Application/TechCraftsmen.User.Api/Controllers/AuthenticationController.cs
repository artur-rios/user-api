using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Configuration.Authorization;
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
        [HttpPost]
        [Route("User")]
        [AllowAnonymous]
        public ActionResult<DataResultDto<AuthenticationToken?>> AuthenticateUser(AuthenticationCredentialsDto credentialsDto)
        {
            OperationResultDto<AuthenticationToken?> authToken = authService.AuthenticateUser(credentialsDto);

            return Resolve(authToken);
        }
    }
}
