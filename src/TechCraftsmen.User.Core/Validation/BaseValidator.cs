using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Core.Validation
{
    public abstract class BaseValidator<T>
    {
        internal readonly SimpleResultDto Result = new();

        public abstract SimpleResultDto Validate(T parameter);
        internal abstract bool IsParameterValid(T parameter, out string message);

        internal SimpleResultDto Resolve()
        {
            Result.Success = !Result.Errors.Any();

            return Result;
        }
    }
}
