using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Data.Relational.Configuration;
using TechCraftsmen.User.Data.Relational.Repositories;

namespace TechCraftsmen.User.Api.Configuration
{
    public static class RelationalDatabaseConfiguration
    {
        public static void AddRelationalContext(this IServiceCollection services, IConfigurationSection configuration)
        {
            services.Configure<RelationalDBContextOptions>(configuration);

            services.AddDbContext<RelationalDBContext>();
            services.AddDbContextFactory<RelationalDBContext>();
        }

        public static void AddRelationalRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICrudRepository<Core.Entities.User>, UserRepository>();
        }
    }
}
