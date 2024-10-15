using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Data.Relational.Configuration;
using TechCraftsmen.User.Data.Relational.Repositories;

namespace TechCraftsmen.User.WebApi.DependencyInjection
{
    public static class RelationalDatabaseConfiguration
    {
        public static void AddRelationalContext(this IServiceCollection services, IConfigurationSection configuration)
        {
            services.Configure<RelationalDbContextOptions>(configuration);

            services.AddDbContext<RelationalDbContext>(optionsLifetime: ServiceLifetime.Singleton);
            services.AddDbContextFactory<RelationalDbContext>();
        }

        public static void AddRelationalRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICrudRepository<Core.Entities.User>, UserRepository>();
        }
    }
}
