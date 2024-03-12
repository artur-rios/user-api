using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Core.Rules
{
    public abstract class BaseRule<T>
    {
        internal RuleResultDto _result = new();

        public abstract RuleResultDto Execute(T parameter);
        internal abstract bool IsParameterValid(T parameter, out string message);

        internal RuleResultDto Resolve()
        {
            _result.Success = !_result.Errors.Any();

            return _result;
        }
    }
}
