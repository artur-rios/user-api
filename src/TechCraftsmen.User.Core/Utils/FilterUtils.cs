namespace TechCraftsmen.User.Core.Utils
{
    public static class FilterUtils
    {
        public static Dictionary<string, object> Create(string key, object value)
        {
            return new Dictionary<string, object>()
            {
                {key, value }
            };
        }
    }
}
