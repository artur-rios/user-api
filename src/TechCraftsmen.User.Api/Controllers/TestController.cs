using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Api.Authorization;
using TechCraftsmen.User.Api.Controllers;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;

namespace TechCraftsmen.User.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class TestController : BaseController
    {
        public TestController(ILogger<TestController> logger) : base(logger)
        {
        }

        [HttpGet]
        [Route("")]
        [RoleRequirement(Roles.ADMIN | Roles.REGULAR)]
        public ActionResult<ResultDto<string>> HelloWorld()
        {
            return Success("Hello world!", "User Api ON");
        }
    }
}
