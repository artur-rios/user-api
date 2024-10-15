using Microsoft.AspNetCore.Mvc;
using TechCraftsmen.User.Core.ValueObjects;
using TechCraftsmen.User.WebApi.Mapping;
using TechCraftsmen.User.WebApi.ValueObjects;
using Results = TechCraftsmen.User.Core.Enums.Results;

namespace TechCraftsmen.User.WebApi.Controllers
{
    public abstract class BaseController : Controller
    {
        protected static ActionResult<WebApiOutput<T>> Resolve<T>(ServiceOutput<T> operationResult)
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
