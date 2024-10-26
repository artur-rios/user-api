using TechCraftsmen.User.Services.Output;
using TechCraftsmen.User.WebApi.Controllers;
using Results = TechCraftsmen.User.Services.Enums.Results;

namespace TechCraftsmen.User.WebApi.Mapping
{
    public static class OutputMapping
    {
        public static BaseController.Output<T?> ToWebApiOutput<T>(this ServiceOutput<T> operationResult)
        {
            bool success = operationResult.Result is Results.Success or Results.Created or Results.EntityNotChanged;

            return new BaseController.Output<T?>(operationResult.Data, operationResult.Messages, success);
        }
    }
}
