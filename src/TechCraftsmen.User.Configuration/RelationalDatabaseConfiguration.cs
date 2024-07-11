using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Data.Relational.Configuration;
using TechCraftsmen.User.Data.Relational.Repositories;

namespace TechCraftsmen.User.Configuration
{
    public static class RelationalDatabaseConfiguration
    {
        public static void AddRelationalContext(this IServiceCollection services, IConfigurationSection configuration)
        {
            services.Configure<RelationalDBContextOptions>(configuration);

            services.AddDbContext<RelationalDBContext>(optionsLifetime: ServiceLifetime.Singleton);
            services.AddDbContextFactory<RelationalDBContext>();
        }

        public static void AddRelationalRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICrudRepository<Core.Entities.User>, UserRepository>();
        }
    }
}
