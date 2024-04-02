using FluentValidation;
using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Core.Validation
{
    public class AuthenticationCredentialsDtoValidator : AbstractValidator<AuthenticationCredentialsDto>
    {
        public AuthenticationCredentialsDtoValidator()
        {
            RuleFor(authCredentials => authCredentials.Email).NotEmpty();
            RuleFor(authCredentials => authCredentials.Password).NotEmpty();
        }
    }
}
