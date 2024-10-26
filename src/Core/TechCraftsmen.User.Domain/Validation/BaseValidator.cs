using TechCraftsmen.User.Domain.Output;

namespace TechCraftsmen.User.Domain.Validation
{
    public abstract class BaseValidator<T>
    {
        internal readonly List<string> Errors = [];
        
        public abstract ProcessOutput Validate(T parameter);
        internal abstract bool IsParameterValid(T parameter, out string message);

        internal ProcessOutput Resolve()
        {
            return new ProcessOutput(Errors);
        }
    }
}
