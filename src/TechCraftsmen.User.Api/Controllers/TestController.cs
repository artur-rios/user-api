using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Api.Controllers;
using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : BaseController
    {
        public TestController(ILogger<TestController> logger) : base(logger)
        {
        }

        [HttpGet]
        [Route("")]
        public ActionResult<ResultDto<string>> HelloWorld()
        {
            return Success("Hello world!", "User Api ON");
        }
    }
}
