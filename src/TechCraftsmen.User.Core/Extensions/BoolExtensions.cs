namespace TechCraftsmen.User.Core.Extensions
{
    public static class BoolExtensions
    {
        public static string ToCustomString(this bool value, string trueString, string falseString)
        {
            return value ? trueString : falseString;
        }
    }
}
