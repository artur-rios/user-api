using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Exceptions;

namespace TechCraftsmen.User.Api.Controllers
{
    public abstract class BaseController(ILogger<BaseController> logger) : Controller
    {
        private readonly ILogger<BaseController> _logger = logger;

        public ActionResult<ResultDto<T>> BadRequest<T>(Exception exception)
        {
            IList<string> errorList = [];

            if (exception is ValidationException)
            {
                var validationException = exception as ValidationException;

                if (validationException is not null)
                {
                    foreach (var error in validationException.Errors)
                    {
                        errorList.Add(error.ErrorMessage);
                    }
                }
            }
            else
            {
                errorList.Add(exception.Message);
            }

            var result = new ResultDto<T>(default, string.Join("|", errorList), false);

            return BadRequest(result);
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
#pragma warning disable CA2254 // Template should be a static expression
                _logger.LogError(exception.InnerException.Message);
#pragma warning restore CA2254 // Template should be a static expression
            }

            var result = new ResultDto<T>(default, message, false);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status500InternalServerError };
        }

        public ActionResult<ResultDto<T>> NoContent<T>(string message)
        {
            var result = new ResultDto<T>(default, message, false);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status204NoContent };
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
