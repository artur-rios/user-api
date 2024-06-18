﻿namespace TechCraftsmen.User.Core.Extensions
{
    public static class StringExtensions
    {
        public static int? ToInt(this string value)
        {
            if (int.TryParse(value, out int result))
            {
                return result;
            }

            return null;
        }

        public static DateTime? ToDateTime(this string value)
        {
            if (DateTime.TryParse(value, out DateTime result))
            {
                return result;
            }

            return null;
        }

        public static bool? ToBool(this string value)
        {
            if (value.Equals("true", StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }

            if (value.Equals("false", StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            return null;
        }
    }
}
