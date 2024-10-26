using TechCraftsmen.User.Data.Configuration;
using TechCraftsmen.User.Data.Repositories;
using TechCraftsmen.User.Domain.Interfaces;

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
            services.AddScoped<ICrudRepository<Domain.Entities.User>, UserRepository>();
        }
    }
}
