using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.WebApi.Authorization;
using TechCraftsmen.User.Domain.Enums;
using TechCraftsmen.User.Services.Filters;
using TechCraftsmen.User.Services.Output;

namespace TechCraftsmen.User.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class HealthCheckController : BaseController
    {
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public ActionResult<Output<string>> HelloWorld()
        {
            ServiceOutput<string> result = new("Hello world!", ["User Api ON"]);
            
            return Resolve(result);
        }

        [HttpGet]
        [Route("Query")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<Output<UserFilter>> TestQuery([FromQuery] UserFilter filter)
        {
            ServiceOutput<UserFilter> result = new(filter, ["Filter parsed with success"]);
            
            return Resolve(result);
        }
    }
}
