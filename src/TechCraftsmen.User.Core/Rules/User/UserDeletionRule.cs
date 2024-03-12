using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Core.Rules.User
{
    public class UserDeletionRule : BaseRule<bool>
    {
        public override RuleResultDto Execute(bool userActive)
        {
            if (!IsParameterValid(userActive, out string validationMessage))
            {
                _result.Errors.Add(validationMessage);
            }

            if (userActive)
            {
                _result.Errors.Add("Can't delete active user!");
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
