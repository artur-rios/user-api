using FluentValidation;
using TechCraftsmen.User.Services.Configuration;

namespace TechCraftsmen.User.Services.Validation
{
    public class AuthenticationTokenConfigurationValidator : AbstractValidator<AuthenticationTokenConfiguration>
    {
        public AuthenticationTokenConfigurationValidator()
        {
            RuleFor(config => config.Audience).NotEmpty();
            RuleFor(config => config.Issuer).NotEmpty();
            RuleFor(config => config.Seconds).NotEmpty().GreaterThan(0);
            RuleFor(config => config.Secret).NotEmpty();
        }
    }
}
