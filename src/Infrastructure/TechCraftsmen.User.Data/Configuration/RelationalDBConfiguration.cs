using TechCraftsmen.User.Domain.Entities;

namespace TechCraftsmen.User.Data.Configuration
{
    internal static class RelationalDbConfiguration
    {
        internal static IDictionary<string, string> Tables
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { nameof(Role), "tb_role" },
                    { nameof(Domain.Entities.User), "tb_user" }
                };
            }
        }
    }
}
