using TechCraftsmen.User.Core.Filters;

namespace TechCraftsmen.User.Api;

public static class FilterConfiguration
{
    public static void AddFilterValidators(this IServiceCollection services)
    {
        services.AddScoped<UserFilterValidator>();
    }
}
