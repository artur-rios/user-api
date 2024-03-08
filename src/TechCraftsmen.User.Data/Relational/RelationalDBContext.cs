using Microsoft.EntityFrameworkCore;
using TechCraftsmen.User.Core.Entities;
using TechCraftsmen.User.Data.Relational.Mapping;

namespace TechCraftsmen.User.Data.Relational
{
    public class RelationalDBContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<Core.Entities.User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().Configure();
            modelBuilder.Entity<Core.Entities.User>().Configure();
        }
    }
}
