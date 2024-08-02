using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;

namespace TechCraftsmen.User.Core.Validation
{
    public class RoleIdValidator : BaseValidator<int>
    {
        public override SimpleResultDto Validate(int roleId)
        {
            if (!IsParameterValid(roleId, out string validationMessage))
            {
                Result.Errors.Add(validationMessage);

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
                Result.Errors.Add("Role should be valid");
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
