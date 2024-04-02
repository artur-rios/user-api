﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechCraftsmen.User.Core.Entities;
using TechCraftsmen.User.Data.Relational.Mapping;

namespace TechCraftsmen.User.Data.Relational.Configuration
{
    public class RelationalDBContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly RelationalDBContextOptions _options;

        public RelationalDBContext(ILoggerFactory loggerFactory, IOptions<RelationalDBContextOptions> options)
        {
            _loggerFactory = loggerFactory;
            _options = options.Value;
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Core.Entities.User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_options.RelationalDatabase);

            optionsBuilder
                .UseLoggerFactory(_loggerFactory)
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
