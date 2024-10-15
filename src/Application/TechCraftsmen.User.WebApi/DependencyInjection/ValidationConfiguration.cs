using TechCraftsmen.User.Core.Validation;

namespace TechCraftsmen.User.WebApi.DependencyInjection
{
    public static class ValidationConfiguration
    {
        public static void AddDomainRules(this IServiceCollection services)
        {
            services.AddScoped<PasswordValidator>();
            services.AddScoped<RoleIdValidator>();
        }
    }
}
