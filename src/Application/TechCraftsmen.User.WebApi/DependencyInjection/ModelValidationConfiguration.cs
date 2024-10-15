using FluentValidation;
using TechCraftsmen.User.Core.Configuration;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Validation.Fluent;

namespace TechCraftsmen.User.WebApi.DependencyInjection
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
