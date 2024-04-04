using System.Text.RegularExpressions;
using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Core.Rules.Email
{
    public partial class EmailRule : BaseRule<string>
    {
        private static readonly Regex _emailRegex = EmailRegex();

        [GeneratedRegex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$")]
        private static partial Regex EmailRegex();

        public override RuleResultDto Execute(string email)
        {
            if (!IsParameterValid(email, out string validationMessage))
            {
                _result.Errors.Add(validationMessage);
            }

            if (!_emailRegex.IsMatch(email))
            {
                _result.Errors.Add("Email should be valid");
            }

            return Resolve();
        }

        internal override bool IsParameterValid(string parameter, out string message)
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
