using Newtonsoft.Json;
using System.Net.Mime;
using System.Reflection;
using System.Text;

namespace TechCraftsmen.User.Utils.Extensions
{
    public static class ObjectExtensions
    {
        public static StringContent ToJsonStringContent(this object @object)
        {
            string json = JsonConvert.SerializeObject(@object);

            return new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        }
        
        public static Dictionary<string, object> ToDictionary(this object @object, string key)
        {
            Dictionary<string, object> dictionary = new() { [key] = @object };

            return dictionary;
        }

        public static Dictionary<string, object> NonNullPropertiesToDictionary(this object @object)
        {
            Dictionary<string, object> dictionary = new();

            foreach (PropertyInfo propertyInfo in @object.GetType().GetProperties())
            {
                object? value = propertyInfo.GetValue(@object);

                if (value is not null)
                {
                    dictionary[propertyInfo.Name] = value;
                }
            }

            return dictionary;
        }
    }
}
