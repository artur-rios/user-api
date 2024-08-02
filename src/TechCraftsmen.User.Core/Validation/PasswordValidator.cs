using TechCraftsmen.User.Core.Collections;
using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Core.Validation
{
    public class PasswordValidator : BaseValidator<string>
    {
        public const int MINIMUM_LENGTH = 8;
        public override SimpleResultDto Validate(string password)
        {
            if (!IsParameterValid(password, out string validationMessage))
            {
                Result.Errors.Add(validationMessage);

                return Resolve();
            }

            if (!RegexCollection.HasNumber().IsMatch(password))
            {
                Result.Errors.Add("Password must contain a number");
            }

            if (!RegexCollection.HasLowerChar().IsMatch(password))
            {
                Result.Errors.Add("Password must contain a lower char");
            }

            if (!RegexCollection.HasUpperChar().IsMatch(password))
            {
                Result.Errors.Add("Password must contain a upper char");
            }

            if (password.Length < MINIMUM_LENGTH)
            {
                Result.Errors.Add($"Password must contain at least {MINIMUM_LENGTH} characters");
            }

            return Resolve();
        }

        internal override bool IsParameterValid(string parameter, out string message)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                message = $"Parameter null or empty";

                return false;
            }

            message = "Parameter valid";

            return true;
        }
    }
}
