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
            OperationResultDto<string> result = new("Hello world!", ["User Api ON"]);
            
            return Resolve(result);
        }

        [HttpGet]
        [Route("Query")]
        [RoleRequirement(Roles.Admin, Roles.Test)]
        public ActionResult<DataResultDto<UserFilter>> TestQuery([FromQuery] UserFilter filter)
        {
            OperationResultDto<UserFilter> result = new(filter, ["Filter parsed with success"]);
            
            return Resolve(result);
        }
    }
}
