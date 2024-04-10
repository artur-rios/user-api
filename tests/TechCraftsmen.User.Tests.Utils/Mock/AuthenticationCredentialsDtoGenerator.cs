using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Rules.Password;

namespace TechCraftsmen.User.Tests.Utils.Mock
{
    public class AuthenticationCredentialsDtoGenerator
    {
        private readonly EmailGenerator _emailGenerator = new();
        private readonly RandomStringGenerator _randomStringGenerator = new();

        private string? _email = null;
        private string? _password = null;

        public AuthenticationCredentialsDtoGenerator WithEmail(string email)
        {
            _email = email;

            return this;
        }

        public AuthenticationCredentialsDtoGenerator WithDefaultEmail()
        {
            _email = _emailGenerator.Generate();

            return this;
        }

        public AuthenticationCredentialsDtoGenerator WithPassword(string password)
        {
            _password = password;

            return this;
        }

        public AuthenticationCredentialsDtoGenerator WithRandomPassword()
        {
            _password = _randomStringGenerator.WithLength(PasswordRule.MINIMUM_LENGTH).WithLowerChars().WithUpperChars().WithNumbers().Generate();

            return this;
        }

        public AuthenticationCredentialsDto Generate()
        {
#pragma warning disable CS8601 // Possible null reference assignment. Reason: expected null
            return new AuthenticationCredentialsDto()
            {
                Email = _email,
                Password = _password
            };
#pragma warning restore CS8601 // Possible null reference assignment.
        }
    }
}
