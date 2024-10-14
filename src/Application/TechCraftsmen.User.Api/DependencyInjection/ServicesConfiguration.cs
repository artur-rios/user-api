using Microsoft.Extensions.DependencyInjection;
using TechCraftsmen.User.Core.Interfaces.Services;
using TechCraftsmen.User.Services;

namespace TechCraftsmen.User.Configuration.DependencyInjection
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
