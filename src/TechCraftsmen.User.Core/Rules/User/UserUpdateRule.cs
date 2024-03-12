using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Core.Rules.User
{
    public class UserUpdateRule : BaseRule<bool>
    {
        public override RuleResultDto Execute(bool userActive)
        {
            if (!IsParameterValid(userActive, out string validationMessage))
            {
                _result.Errors.Add(validationMessage);
            }

            if (userActive)
            {
                _result.Errors.Add("Can't update inactive user!");
            }

            return Resolve();
        }

        internal override bool IsParameterValid(bool parameter, out string message)
        {
            message = "Bool parameter, there's nothing to validate.";

            return true;
        }
    }
}
