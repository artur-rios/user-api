using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.WebApi.Authorization;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Filters;
using TechCraftsmen.User.Core.ValueObjects;
using TechCraftsmen.User.WebApi.ValueObjects;

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
        public ActionResult<WebApiOutput<string>> HelloWorld()
        {
            ServiceOutput<string> result = new("Hello world!", ["User Api ON"]);
            
            return Resolve(result);
        }

        [HttpGet]
        [Route("Query")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<WebApiOutput<UserFilter>> TestQuery([FromQuery] UserFilter filter)
        {
            ServiceOutput<UserFilter> result = new(filter, ["Filter parsed with success"]);
            
            return Resolve(result);
        }
    }
}
