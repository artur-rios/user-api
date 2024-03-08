using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Common.Dto;
using TechCraftsmen.User.Core.Exceptions;

namespace TechCraftsmen.User.Api.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly ILogger<BaseController> _logger;

        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        public ActionResult<ResultDto<T>> Created<T>(T data, string message)
        {
            var result = new ResultDto<T>(data, message, true);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        public ActionResult<ResultDto<T>> Error<T>(Exception exception, string message = "Internal error, please try again later")
        {
            _logger.LogError(exception.Message);

            if (exception.InnerException is not null)
            {
                _logger.LogError(exception.InnerException.Message);
            }

            var result = new ResultDto<T>(default, message, false);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status500InternalServerError };
        }

        public ActionResult<ResultDto<T>> NotFound<T>(NotFoundException exception)
        {
            var result = new ResultDto<T>(default, exception.Message, false);

            return NotFound(result);
        }

        public ActionResult<ResultDto<T>> Success<T>(T data, string message)
        {
            var result = new ResultDto<T>(data, message, true);

            return Ok(result);
        }
    }
}
