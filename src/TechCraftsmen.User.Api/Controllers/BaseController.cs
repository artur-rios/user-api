using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Api.Controllers
{
    public abstract class BaseController : Controller
    {
        public ActionResult<DataResultDto<T>> Created<T>(T data, string message)
        {
            DataResultDto<T> result = new DataResultDto<T>(data, message, true);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        public ActionResult<DataResultDto<T>> NoContent<T>(string message)
        {
            DataResultDto<T> result = new DataResultDto<T>(default, message, false);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status204NoContent };
        }

        public ActionResult<DataResultDto<T>> Success<T>(T data, string message)
        {
            DataResultDto<T> result = new DataResultDto<T>(data, message, true);

            return Ok(result);
        }
    }
}
