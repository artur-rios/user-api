using TechCraftsmen.User.Core.ValueObjects;
using TechCraftsmen.User.WebApi.ValueObjects;
using Results = TechCraftsmen.User.Core.Enums.Results;

namespace TechCraftsmen.User.WebApi.Mapping
{
    public static class OutputMapping
    {
        public static WebApiOutput<T?> ToWebApiOutput<T>(this ServiceOutput<T> operationResult)
        {
            bool success = operationResult.Result is Results.Success or Results.Created or Results.EntityNotChanged;

            return new WebApiOutput<T?>(operationResult.Data, operationResult.Messages, success);
        }
    }
}