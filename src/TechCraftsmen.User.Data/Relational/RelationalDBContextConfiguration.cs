using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TechCraftsmen.User.Data.Relational
{
    public static class RelationalDBContextConfiguration
    {
        public static void AddRelationalDBContext(this IServiceCollection services, IConfigurationSection configuration)
        {
            services.Configure<RelationalDBContextOptions>(configuration);

            services.AddDbContext<RelationalDBContext>();
            services.AddDbContextFactory<RelationalDBContext>();
        }
    }
}
