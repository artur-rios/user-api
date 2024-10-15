using TechCraftsmen.User.Services.Implementations;
using TechCraftsmen.User.Services.Interfaces;

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
