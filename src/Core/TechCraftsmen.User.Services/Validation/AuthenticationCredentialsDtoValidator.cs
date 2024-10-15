using FluentValidation;
using TechCraftsmen.User.Services.Dto;

namespace TechCraftsmen.User.Services.Validation
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
