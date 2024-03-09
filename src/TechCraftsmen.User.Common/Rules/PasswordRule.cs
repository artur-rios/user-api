using System.Text.RegularExpressions;
using TechCraftsmen.User.Common.Dto;
using TechCraftsmen.User.Core.Interfaces.Rules;

namespace TechCraftsmen.User.Common.Rules
{
    public class PasswordRule : IRule
    {
        private const int MINIMUM_LENGTH = 8;

        private static readonly Regex _hasNumber = new(@"[0-9]+");
        private static readonly Regex _hasLowerChar = new(@"[a-z]+");
        private static readonly Regex _hasUpperChar = new(@"[A-Z]+");
        private static readonly Regex _hasMinimumLength = new(@".{" + MINIMUM_LENGTH + ",}");

        public RuleResultDto Execute(params object[] ruleParameters)
        {
            var result = new RuleResultDto();

            if (!ValidateParameters(out string validationResult, ruleParameters))
            {
                result.Errors.Add(validationResult);
            }

            if (_hasNumber.IsMatch(validationResult))
            {
                result.Errors.Add("Password must contain a number");
            }

            if (_hasLowerChar.IsMatch(validationResult))
            {
                result.Errors.Add("Password must contain a lower char");
            }

            if (_hasUpperChar.IsMatch(validationResult))
            {
                result.Errors.Add("Password must contain a upper char");
            }

            if (_hasMinimumLength.IsMatch(validationResult))
            {
                result.Errors.Add($"Password must contain at least {MINIMUM_LENGTH} characters");
            }

            result.Success = !result.Errors.Any();

            return result;
        }

        private bool ValidateParameters(out string result, params object[] ruleParameters)
        {
            if (ruleParameters.Length == 0)
            {
                result = "No parameters were passed to rule!";

                return false;
            }

            if (ruleParameters.Length > 1)
            {
                result = $"{nameof(PasswordRule)} only accepts one parameter.";

                return false;
            }

            var parameter = ruleParameters[0];

            if (parameter is not string)
            {
                result = $"A string must be passed to the rule.";

                return false;
            }

            var password = parameter as string;

            if (string.IsNullOrEmpty(password))
            {
                result = $"Parameter null or empty.";

                return false;
            }

            result = password;

            return true;
        }
    }
}
