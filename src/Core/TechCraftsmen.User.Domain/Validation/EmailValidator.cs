using TechCraftsmen.User.Domain.Output;

namespace TechCraftsmen.User.Domain.Validation
{
    public class EmailValidator : BaseValidator<string>
    {
        public override ProcessOutput Validate(string email)
        {
            if (!IsParameterValid(email, out string validationMessage))
            {
                Errors.Add(validationMessage);

                return Resolve();
            }

            if (!RegexCollection.Email().IsMatch(email))
            {
                Errors.Add("Email should be valid");
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
