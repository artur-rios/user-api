using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Rules.Password;

namespace TechCraftsmen.User.Tests.Utils.Generators
{
    public class AuthenticationCredentialsDtoGenerator
    {
        private readonly EmailGenerator _emailGenerator = new();
        private readonly RandomStringGenerator _randomStringGenerator = new();

        private string _email = string.Empty;
        private string _password = string.Empty;

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
            return new AuthenticationCredentialsDto()
            {
                Email = _email,
                Password = _password
            };
        }
    }
}
