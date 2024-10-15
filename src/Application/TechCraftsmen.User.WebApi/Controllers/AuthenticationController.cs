using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.WebApi.Authorization;
using TechCraftsmen.User.Core.Entities;
using TechCraftsmen.User.Core.ValueObjects;
using TechCraftsmen.User.Services.Dto;
using TechCraftsmen.User.Services.Interfaces;
using TechCraftsmen.User.WebApi.ValueObjects;

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
        public ActionResult<WebApiOutput<AuthenticationToken?>> AuthenticateUser(AuthenticationCredentialsDto credentialsDto)
        {
            ServiceOutput<AuthenticationToken?> authToken = authService.AuthenticateUser(credentialsDto);

            return Resolve(authToken);
        }
    }
}
