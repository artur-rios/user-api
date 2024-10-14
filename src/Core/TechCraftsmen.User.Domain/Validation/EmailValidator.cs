using System.Text.RegularExpressions;
using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Core.Validation
{
    public partial class EmailValidator : BaseValidator<string>
    {
        private static readonly Regex ValidatorRegex = EmailRegex();

        [GeneratedRegex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$")]
        private static partial Regex EmailRegex();
        
        public override SimpleResultDto Validate(string email)
        {
            if (!IsParameterValid(email, out string validationMessage))
            {
                Result.Errors.Add(validationMessage);

                return Resolve();
            }

            if (!ValidatorRegex.IsMatch(email))
            {
                Result.Errors.Add("Email should be valid");
            }

            return Resolve();
        }

        internal override bool IsParameterValid(string parameter, out string message)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                message = "Email must not be null or empty";

                return false;
            }

            message = "Parameter valid";

            return true;
        }
    }
}
