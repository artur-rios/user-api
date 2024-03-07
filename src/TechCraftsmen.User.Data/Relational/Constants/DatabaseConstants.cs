namespace TechCraftsmen.User.Data.Relational.Constants
{
    internal static class DatabaseConstants
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
