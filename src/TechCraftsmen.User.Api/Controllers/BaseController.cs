using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Api.Controllers
{
    public abstract class BaseController : Controller
    {
        public ActionResult<ResultDto<T>> Created<T>(T data, string message)
        {
            var result = new ResultDto<T>(data, message, true);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        public ActionResult<ResultDto<T>> NoContent<T>(string message)
        {
            var result = new ResultDto<T>(default, message, false);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status204NoContent };
        }

        public ActionResult<ResultDto<T>> Success<T>(T data, string message)
        {
            var result = new ResultDto<T>(data, message, true);

            return Ok(result);
        }
    }
}
