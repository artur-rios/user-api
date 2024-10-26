using Newtonsoft.Json;
using System.Net;
using TechCraftsmen.User.Utils.Exceptions;
using TechCraftsmen.User.WebApi.Controllers;

namespace TechCraftsmen.User.WebApi.Middleware
{

    public class ExceptionHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger("ExceptionHandler");

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleException(httpContext, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            string[] messages = ["Internal server error, please try again later"];
            
            if (exception is CustomException customException)
            {
                messages = customException.Messages;
            }
            
            BaseController.Output<string> response = new(string.Empty, messages);
            const HttpStatusCode httpStatus = HttpStatusCode.InternalServerError;
            
            _logger.LogError("Error: {error}", exception.Message);

            foreach (string message in messages)
            {
                _logger.LogError("Message: {message}", message);
            }
            
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
