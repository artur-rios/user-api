using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Entities;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Interfaces.Services;

namespace TechCraftsmen.User.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(ILogger<BaseController> logger, IAuthenticationService authService) : base(logger)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("User")]
        [AllowAnonymous]
        public ActionResult<ResultDto<AuthenticationToken>> AuthenticateUser(AuthenticationCredentialsDto credentialsDto)
        {
            try
            {
                var authToken = _authService.AuthenticateUser(credentialsDto);

                return Success(authToken, "User authenticated with success");
            }
            catch (NotAllowedException nae)
            {
                return BadRequest<AuthenticationToken>(nae);
            }
            catch (NotFoundException nfe)
            {
                return NotFound<AuthenticationToken>(nfe);
            }
            catch (ValidationException ve)
            {
                return BadRequest<AuthenticationToken>(ve);
            }
            catch (Exception ex)
            {
                return Error<AuthenticationToken>(ex);
            }
        }
    }
}
