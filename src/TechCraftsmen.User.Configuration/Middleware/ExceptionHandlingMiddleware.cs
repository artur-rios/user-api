using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Exceptions;

namespace TechCraftsmen.User.Configuration.Middleware
{

    public class ExceptionHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger _logger = loggerFactory.CreateLogger("ExceptionHandler");

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleException(httpContext, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            ResultDto<string> response;
            HttpStatusCode httpStatus;

            if (exception is NotAllowedException)
            {
                response = new ResultDto<string>("The request was unsuccessful", exception.Message, false);
                httpStatus = HttpStatusCode.BadRequest;
            }
            else if (exception is NotFoundException)
            {
                response = new ResultDto<string>("The resource request was not found", exception.Message, false);
                httpStatus = HttpStatusCode.NotFound;
            }
            else if (exception is ValidationException)
            {
                IList<string> errorList = [];

                var validationException = exception as ValidationException;

                if (validationException is not null)
                {
                    foreach (var error in validationException.Errors)
                    {
                        errorList.Add(error.ErrorMessage);
                    }
                }

                response = new ResultDto<string>("Validation error", string.Join(" | ", errorList), false);
                httpStatus = HttpStatusCode.BadRequest;
            }
            else
            {
                response = new ResultDto<string>("An error has ocurred", "Internal error, please try again later", false);
                httpStatus = HttpStatusCode.InternalServerError;
            }

            _logger.LogError("Error: {error}", exception.Message);
            _logger.LogError("Stack: {stack}", exception.StackTrace);

            if (exception.InnerException is not null)
            {
                _logger.LogError("Inner exception on request: {message}", exception.InnerException.Message);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatus;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
