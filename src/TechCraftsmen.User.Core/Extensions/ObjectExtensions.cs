using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace TechCraftsmen.User.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsFilledString(this object value)
        {
            return value is string && !string.IsNullOrEmpty(value as string);
        }

        public static bool IsIntBiggerThanZero(this object value)
        {
            return value is int intValue && intValue > 0;
        }

        public static bool IsPastDateTime(this object value)
        {
            return value is DateTime datetime && datetime < DateTime.UtcNow;
        }

        public static StringContent ToJsonContent(this object @object)
        {
            var json = JsonSerializer.Serialize(@object);

            return new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        }
    }
}
