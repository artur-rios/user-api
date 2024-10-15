using TechCraftsmen.User.Core.Interfaces.Services;
using TechCraftsmen.User.Services;

namespace TechCraftsmen.User.WebApi.DependencyInjection
{
    public static class ServicesConfiguration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
