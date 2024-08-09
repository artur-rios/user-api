using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;

namespace TechCraftsmen.User.Core.Mapping
{
    public static class ResultMapping
    {
        public static DataResultDto<T?> ToDataResult<T>(this OperationResultDto<T> operationResult)
        {
            bool success = operationResult.Result is Results.Success or Results.Created or Results.EntityNotChanged;

            return new DataResultDto<T?>(operationResult.Data, operationResult.Messages, success);
        }
    }
}