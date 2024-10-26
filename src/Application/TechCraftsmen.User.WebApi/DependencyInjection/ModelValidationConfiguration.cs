using FluentValidation;
using TechCraftsmen.User.Services.Configuration;
using TechCraftsmen.User.Services.Dto;
using TechCraftsmen.User.Services.Validation;

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
