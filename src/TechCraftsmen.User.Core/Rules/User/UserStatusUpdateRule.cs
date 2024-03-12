using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Utils;

namespace TechCraftsmen.User.Core.Rules.User
{
    public class UserStatusUpdateRule : BaseRule<Tuple<bool, bool>>
    {
        public override RuleResultDto Execute(Tuple<bool, bool> parameter)
        {
            if (!IsParameterValid(parameter, out string validationMessage))
            {
                _result.Errors.Add(validationMessage);
            }

            parameter.Deconstruct(out bool actualStatus, out bool newStatus);

            if (actualStatus == newStatus)
            {
                _result.Errors.Add($"User already {actualStatus.ToCustomString("active", "inactive")}!");
            }

            return Resolve();
        }

        internal override bool IsParameterValid(Tuple<bool, bool> parameter, out string message)
        {
            message = "Bool parameter, there's nothing to validate.";

            return true;
        }
    }
}
