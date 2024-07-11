using Microsoft.Extensions.DependencyInjection;
using TechCraftsmen.User.Core.Filters;

namespace TechCraftsmen.User.Configuration
{
    public static class FilterConfiguration
    {
        public static void AddFilterValidators(this IServiceCollection services)
        {
            services.AddScoped<UserFilterValidator>();
        }
    }
}
