using System.Text.RegularExpressions;

namespace TechCraftsmen.User.Domain.Validation
{
    public static partial class RegexCollection
    {
        [GeneratedRegex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$")]
        public static partial Regex Email();
        
        [GeneratedRegex(@"[0-9]+")]
        public static partial Regex HasNumber();

        [GeneratedRegex(@"[a-z]+")]
        public static partial Regex HasLowerChar();

        [GeneratedRegex(@"[A-Z]+")]
        public static partial Regex HasUpperChar();
    }
}
