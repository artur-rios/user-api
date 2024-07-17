using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : BaseController
    {
        [HttpGet]
        [Route("")]
        public ActionResult<ResultDto<string>> HelloWorld()
        {
            return Success("Hello world!", "User Api ON");
        }
    }
}
