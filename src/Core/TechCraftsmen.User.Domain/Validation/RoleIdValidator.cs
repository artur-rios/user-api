using TechCraftsmen.User.Domain.Enums;
using TechCraftsmen.User.Domain.Output;

namespace TechCraftsmen.User.Domain.Validation
{
    public class RoleIdValidator : BaseValidator<int>
    {
        public override ProcessOutput Validate(int roleId)
        {
            if (!IsParameterValid(roleId, out string validationMessage))
            {
                Errors.Add(validationMessage);

                return Resolve();
            }

            bool validRole = Enum.GetValues(typeof(Roles)).Cast<Roles>().Any(role => roleId == (int)role);

            if (!validRole)
            {
                Errors.Add("Role should be valid");
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
