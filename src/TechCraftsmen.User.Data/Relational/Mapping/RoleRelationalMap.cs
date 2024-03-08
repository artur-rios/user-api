using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechCraftsmen.User.Core.Entities;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Data.Relational.Constants;

namespace TechCraftsmen.User.Data.Relational.Mapping
{
    internal static class RoleRelationalMap
    {
        public static void Configure(this EntityTypeBuilder<Role> role)
        {
            DatabaseConstants.Tables.TryGetValue(nameof(Role), out string? tableName);

            if (tableName is null)
            {
                throw new NotFoundException("Entity not mapped to relational table!");
            }

            role.ToTable(tableName, schema: "sc_user_api");

            role.HasKey(r => r.Id);

            role.Property(r => r.Id).HasColumnName("id");
            role.Property(r => r.Name).HasColumnName("name");
            role.Property(r => r.Description).HasColumnName("description");
        }
    }
}
