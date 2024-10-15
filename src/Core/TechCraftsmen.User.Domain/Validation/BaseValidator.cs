using TechCraftsmen.User.Core.ValueObjects;

namespace TechCraftsmen.User.Core.Validation
{
    public abstract class BaseValidator<T>
    {
        internal readonly DomainOutput Result = new();

        public abstract DomainOutput Validate(T parameter);
        internal abstract bool IsParameterValid(T parameter, out string message);

        internal DomainOutput Resolve()
        {
            return Result;
        }
    }
}
