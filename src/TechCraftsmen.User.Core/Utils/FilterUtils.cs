using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace TechCraftsmen.User.Core.Utils
{
    public static class FilterUtils
    {
        public static Dictionary<string, object> CreateDictionary(string key, object value)
        {
            return new Dictionary<string, object>()
            {
                { key, value }
            };
        }
    }
}
