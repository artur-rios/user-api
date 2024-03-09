using System.Text.RegularExpressions;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Interfaces.Rules;

namespace TechCraftsmen.User.Core.Rules
{
    public class PasswordRule : IRule<string>
    {
        private const int MINIMUM_LENGTH = 8;

        private static readonly Regex _hasNumber = new(@"[0-9]+");
        private static readonly Regex _hasLowerChar = new(@"[a-z]+");
        private static readonly Regex _hasUpperChar = new(@"[A-Z]+");
        private static readonly Regex _hasMinimumLength = new(@".{" + MINIMUM_LENGTH + ",}");

        public RuleResultDto Execute(string parameter)
        {
            var result = new RuleResultDto();

            if (!IsParameterValid(parameter, out string validationMessage))
            {
                result.Errors.Add(validationMessage);
            }

            if (!_hasNumber.IsMatch(parameter))
            {
                result.Errors.Add("Password must contain a number");
            }

            if (!_hasLowerChar.IsMatch(parameter))
            {
                result.Errors.Add("Password must contain a lower char");
            }

            if (!_hasUpperChar.IsMatch(parameter))
            {
                result.Errors.Add("Password must contain a upper char");
            }

            if (!_hasMinimumLength.IsMatch(parameter))
            {
                result.Errors.Add($"Password must contain at least {MINIMUM_LENGTH} characters");
            }

            result.Success = !result.Errors.Any();

            return result;
        }

        private static bool IsParameterValid(string parameter, out string message)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                message = $"Parameter null or empty.";

                return false;
            }

            message = "Parameter valid";

            return true;
        }
    }
}
