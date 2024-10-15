using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.ValueObjects;

namespace TechCraftsmen.User.Core.Validation
{
    public class RoleIdValidator : BaseValidator<int>
    {
        public override DomainOutput Validate(int roleId)
        {
            if (!IsParameterValid(roleId, out string validationMessage))
            {
                Result.Errors.Add(validationMessage);

                return Resolve();
            }

            bool validRole = Enum.GetValues(typeof(Roles)).Cast<Roles>().Any(role => roleId == (int)role);

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
