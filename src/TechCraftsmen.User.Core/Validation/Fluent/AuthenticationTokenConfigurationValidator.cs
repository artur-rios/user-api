using FluentValidation;
using TechCraftsmen.User.Core.Configuration;

namespace TechCraftsmen.User.Core.Validation.Fluent
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
