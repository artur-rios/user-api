namespace TechCraftsmen.User.Core.Utils
{
    public static class BoolExtensions
    {
        public static string ToCustomString(this bool value, string trueString, string falseString)
        {
            return value ? trueString : falseString;
        }
    }
}
