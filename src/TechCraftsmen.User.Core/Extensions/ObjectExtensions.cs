namespace TechCraftsmen.User.Core.Extensions;

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
}
