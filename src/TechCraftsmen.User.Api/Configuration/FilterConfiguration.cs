using TechCraftsmen.User.Core.Filters;

namespace TechCraftsmen.User.Api.Configuration
{
    public static class FilterConfiguration
    {
        public static void AddFilterValidators(this IServiceCollection services)
        {
            services.AddScoped<UserFilterValidator>();
        }
    }
}
