using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Common.Dto;

namespace TechCraftsmen.User.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        [Route("HelloWorld")]
        public ResultDto<string> HelloWorld()
        {
            _logger.LogInformation("Hello world! User Api ON");

            return new ResultDto<string>("Hello world!", "User Api ON", true);
        }
    }
}
