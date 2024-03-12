namespace TechCraftsmen.User.Data.Relational.Configuration
{
    internal static class RelationalDBConfiguration
    {
        internal static IDictionary<string, string> Tables
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { nameof(Core.Entities.Role), "tb_role" },
                    { nameof(Core.Entities.User), "tb_user" }
                };
            }
        }
    }
}
