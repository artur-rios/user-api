using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechCraftsmen.User.Data.Mapping;
using TechCraftsmen.User.Domain.Entities;

namespace TechCraftsmen.User.Data.Configuration
{
    public class RelationalDbContext(ILoggerFactory loggerFactory, IOptions<RelationalDbContextOptions> options) : DbContext
    {
        private readonly RelationalDbContextOptions _options = options.Value;

        public DbSet<Role> Roles { get; init; }
        public DbSet<Domain.Entities.User> Users { get; init; }

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
            modelBuilder.Entity<Domain.Entities.User>().Configure();
        }
    }
}
