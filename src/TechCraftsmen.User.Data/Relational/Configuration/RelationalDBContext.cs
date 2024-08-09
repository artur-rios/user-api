using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechCraftsmen.User.Core.Entities;
using TechCraftsmen.User.Data.Relational.Mapping;

namespace TechCraftsmen.User.Data.Relational.Configuration
{
    public class RelationalDbContext(ILoggerFactory loggerFactory, IOptions<RelationalDbContextOptions> options) : DbContext
    {
        private readonly RelationalDbContextOptions _options = options.Value;

        public DbSet<Role> Roles { get; init; }
        public DbSet<Core.Entities.User> Users { get; init; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_options.RelationalDatabase);

            optionsBuilder
                .UseLoggerFactory(loggerFactory)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().Configure();
            modelBuilder.Entity<Core.Entities.User>().Configure();
        }
    }
}
