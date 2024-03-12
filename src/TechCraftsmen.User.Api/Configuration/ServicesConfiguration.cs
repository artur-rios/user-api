using TechCraftsmen.User.Core.Interfaces.Services;
using TechCraftsmen.User.Core.Services.Implementation;

namespace TechCraftsmen.User.Api.Configuration
{
    public static class ServicesConfiguration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}
