using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class HealthCheckController : BaseController
    {
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public ActionResult<ResultDto<string>> HelloWorld()
        {
            return Success("Hello world!", "User Api ON");
        }
    }
}
