using System.Text.RegularExpressions;

namespace TechCraftsmen.User.Core.Utils
{
    public static partial class RegexCollection
    {
        [GeneratedRegex(@"[0-9]+")]
        public static partial Regex HasNumber();

        [GeneratedRegex(@"[a-z]+")]
        public static partial Regex HasLowerChar();

        [GeneratedRegex(@"[A-Z]+")]
        public static partial Regex HasUpperChar();
    }
}
