using FluentValidation;
using TechCraftsmen.User.Core.Configuration;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Validation;

namespace TechCraftsmen.User.Api.Configuration
{
    public static class ModelValidationConfiguration
    {
        public static void AddModelValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<AuthenticationCredentialsDto>, AuthenticationCredentialsDtoValidator>();
            services.AddScoped<IValidator<AuthenticationTokenConfiguration>, AuthenticationTokenConfigurationValidator>();
            services.AddScoped<IValidator<UserDto>, UserDtoValidator>();
        }
    }
}
