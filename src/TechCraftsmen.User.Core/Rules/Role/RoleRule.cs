using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;

namespace TechCraftsmen.User.Core.Rules.Role
{
    public class RoleRule : BaseRule<int>
    {
        public override RuleResultDto Execute(int roleId)
        {
            if (!IsParameterValid(roleId, out string validationMessage))
            {
                _result.Errors.Add(validationMessage);

                return Resolve();
            }

            var validRole = false;

            foreach (Roles role in Enum.GetValues(typeof(Roles)))
            {
                if (roleId == (int)role)
                {
                    validRole = true;
                    break;
                }
            }

            if (!validRole)
            {
                _result.Errors.Add("Email should be valid");
            }

            return Resolve();
        }

        internal override bool IsParameterValid(int parameter, out string message)
        {
            if (parameter <= 0)
            {
                message = "RoleId must be greater than zero";

                return false;
            }

            message = "Parameter valid";

            return true;
        }
    }
}
