using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Api.Authorization;
using TechCraftsmen.User.Api.Controllers;
using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class HealthCheckController(ILogger<HealthCheckController> logger) : BaseController(logger)
    {
        [HttpGet]
        [Route("")]
        public ActionResult<ResultDto<string>> HelloWorld()
        {
            return Success("Hello world!", "User Api ON");
        }
    }
}
