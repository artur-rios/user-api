using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.WebApi.Authorization;
using TechCraftsmen.User.Services.Authentication;
using TechCraftsmen.User.Services.Dto;
using TechCraftsmen.User.Services.Interfaces;
using TechCraftsmen.User.Services.Output;

namespace TechCraftsmen.User.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AuthenticationController(IAuthenticationService authService) : BaseController
    {
        [HttpPost]
        [Route("User")]
        [AllowAnonymous]
        public ActionResult<Output<AuthenticationToken?>> AuthenticateUser(AuthenticationCredentialsDto credentialsDto)
        {
            ServiceOutput<AuthenticationToken?> authToken = authService.AuthenticateUser(credentialsDto);

            return Resolve(authToken);
        }
    }
}
