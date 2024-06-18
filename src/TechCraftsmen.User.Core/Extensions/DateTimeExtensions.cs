namespace TechCraftsmen.User.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime TrimMilliseconds(this DateTime dateTime)
        {
            return dateTime.AddTicks(-dateTime.Ticks % TimeSpan.TicksPerSecond);
        }
    }
}
