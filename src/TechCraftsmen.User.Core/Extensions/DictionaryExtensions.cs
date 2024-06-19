using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace TechCraftsmen.User.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static QueryCollection ToQueryCollection(this Dictionary<string, string> values)
        {
            Dictionary<string, StringValues> dict = [];

            foreach (var entry in values)
            {
                dict.Add(entry.Key, entry.Value);
            }

            return new QueryCollection(dict);
        }
    }
}
