using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechCraftsmen.User.Core.Entities;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Data.Relational.Configuration;

namespace TechCraftsmen.User.Data.Relational.Mapping
{
    public static class UserRelationalMap
    {
        public static void Configure(this EntityTypeBuilder<Core.Entities.User> user)
        {
            RelationalDbConfiguration.Tables.TryGetValue(nameof(Core.Entities.User), out string? tableName);

            if (tableName is null)
            {
                throw new CustomException(["Entity not mapped to relational table"]);
            }

            user.ToTable(tableName, schema: "sc_user_api");

            user.HasKey(u => u.Id);

            user.HasOne<Role>()
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .IsRequired();

            user.Property(u => u.Id).HasColumnName("id");
            user.Property(u => u.Name).HasColumnName("name");
            user.Property(u => u.Email).HasColumnName("email");
            user.Property(u => u.Password).HasColumnName("password");
            user.Property(u => u.Salt).HasColumnName("salt");
            user.Property(u => u.RoleId).HasColumnName("role_id");
            user.Property(u => u.CreatedAt).HasColumnName("created_at");
            user.Property(u => u.Active).HasColumnName("active");
        }
    }
}
