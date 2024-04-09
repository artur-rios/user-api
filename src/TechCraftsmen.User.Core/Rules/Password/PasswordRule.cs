using System.Text.RegularExpressions;
using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Core.Rules.Password
{
    public partial class PasswordRule : BaseRule<string>
    {
        public const int MINIMUM_LENGTH = 8;

        private static readonly Regex _hasNumber = HasNumber();
        private static readonly Regex _hasLowerChar = HasLowerChar();
        private static readonly Regex _hasUpperChar = HasUpperChar();
        private static readonly Regex _hasMinimumLength = new(@".{" + MINIMUM_LENGTH + ",}");

        [GeneratedRegex(@"[0-9]+")]
        private static partial Regex HasNumber();

        [GeneratedRegex(@"[a-z]+")]
        private static partial Regex HasLowerChar();

        [GeneratedRegex(@"[A-Z]+")]
        private static partial Regex HasUpperChar();

        public override RuleResultDto Execute(string password)
        {
            if (!IsParameterValid(password, out string validationMessage))
            {
                _result.Errors.Add(validationMessage);

                return Resolve();
            }

            if (!_hasNumber.IsMatch(password))
            {
                _result.Errors.Add("Password must contain a number");
            }

            if (!_hasLowerChar.IsMatch(password))
            {
                _result.Errors.Add("Password must contain a lower char");
            }

            if (!_hasUpperChar.IsMatch(password))
            {
                _result.Errors.Add("Password must contain a upper char");
            }

            if (!_hasMinimumLength.IsMatch(password))
            {
                _result.Errors.Add($"Password must contain at least {MINIMUM_LENGTH} characters");
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
