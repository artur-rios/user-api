using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Services.Output;
using TechCraftsmen.User.WebApi.Mapping;
using Results = TechCraftsmen.User.Services.Enums.Results;

namespace TechCraftsmen.User.WebApi.Controllers
{
    public abstract class BaseController : Controller
    {
        public class Output<T>(T data, string[] messages, bool success = false)
        {
            public T Data { get; } = data;
            public string[] Messages { get; } = messages;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
            public bool Success { get; set; } = success;
        }
        
        protected static ActionResult<Output<T>> Resolve<T>(ServiceOutput<T> operationResult)
        {
            return new ObjectResult(operationResult.ToWebApiOutput()){ StatusCode = ToStatusCode(operationResult.Result) };
        }

        private static int ToStatusCode(Results result) => result switch
        {
            Results.Created => StatusCodes.Status201Created,
            Results.InternalError => StatusCodes.Status500InternalServerError,
            Results.NotAllowed => StatusCodes.Status400BadRequest,
            Results.NotFound => StatusCodes.Status404NotFound,
            Results.EntityNotChanged => StatusCodes.Status204NoContent,
            Results.ValidationError => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status200OK
        };
    }
}
