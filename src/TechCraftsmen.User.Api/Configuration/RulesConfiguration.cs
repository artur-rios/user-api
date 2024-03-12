using TechCraftsmen.User.Core.Rules.Password;
using TechCraftsmen.User.Core.Rules.User;

namespace TechCraftsmen.User.Api.Configuration
{
    public static class RulesConfiguration
    {
        public static void AddDomainRules(this IServiceCollection services)
        {
            services.AddScoped<PasswordRule>();
            services.AddScoped<UserCreationRule>();
            services.AddScoped<UserUpdateRule>();
        }
    }
}
