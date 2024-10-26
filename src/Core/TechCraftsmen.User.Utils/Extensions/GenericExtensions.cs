using Newtonsoft.Json;

namespace TechCraftsmen.User.Utils.Extensions
{
    public static class GenericExtensions
    {
        public static T? Clone<T>(this T source)
        {
            string serialized = JsonConvert.SerializeObject(source);
            
            return JsonConvert.DeserializeObject<T>(serialized);
        }
    }
}
