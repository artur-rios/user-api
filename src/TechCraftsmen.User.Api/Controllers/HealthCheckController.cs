using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Configuration.Authorization;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Filters;

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
        public ActionResult<DataResultDto<string>> HelloWorld()
        {
            return Success("Hello world!", "User Api ON");
        }

        [HttpGet]
        [Route("Query")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<DataResultDto<UserFilter>> TestQuery([FromQuery] UserFilter filter)
        {
            return Success(filter, "Filter parsed with success");
        }
    }
}
