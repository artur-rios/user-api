using Microsoft.Extensions.DependencyInjection;
using TechCraftsmen.User.Core.Rules.Password;
using TechCraftsmen.User.Core.Rules.Role;
using TechCraftsmen.User.Core.Rules.User;

namespace TechCraftsmen.User.Configuration.DependencyInjection
{
    public static class RulesConfiguration
    {
        public static void AddDomainRules(this IServiceCollection services)
        {
            services.AddScoped<PasswordRule>();
            services.AddScoped<RoleRule>();
            services.AddScoped<UserCreationRule>();
            services.AddScoped<UserUpdateRule>();
            services.AddScoped<UserStatusUpdateRule>();
            services.AddScoped<UserDeletionRule>();
        }
    }
}
