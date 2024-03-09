using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Interfaces.Rules;

namespace TechCraftsmen.User.Core.Rules
{
    public class UserUpdateRule : IRule<Entities.User>
    {
        public RuleResultDto Execute(Entities.User parameter)
        {
            var result = new RuleResultDto();

            if (!IsParameterValid(parameter, out string validationMessage))
            {
                result.Errors.Add(validationMessage);
            }

            if (!parameter.Active)
            {
                result.Errors.Add("Can't update inactive user!");
            }

            result.Success = !result.Errors.Any();

            return result;
        }

        private bool IsParameterValid(Entities.User parameter, out string message)
        {
            if (parameter is null)
            {
                message = "User can't be null";

                return false;
            }

            message = "Valid parameter";

            return true;
        }
    }
}
