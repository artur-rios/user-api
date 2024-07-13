using System.Text;
using TechCraftsmen.User.Core.Collections;
using TechCraftsmen.User.Core.Exceptions;

namespace TechCraftsmen.User.Tests.Utils.Generators
{
    public class RandomStringGenerator
    {
        private const string LOWER_CHARS = "abcdefghijklmnopqrstuvwxyz";
        private const string UPPER_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string NUMBERS = "0123456789";

        private int _length;
        private bool _lowerChars;
        private bool _upperChars;
        private bool _numbers;

        private string[] _excludedStrings = [];

        public RandomStringGenerator DifferentFrom(string[] excludedStrings)
        {
            _excludedStrings = excludedStrings;

            return this;
        }

        public RandomStringGenerator WithLength(int length)
        {
            _length = length;

            return this;
        }

        public RandomStringGenerator WithLowerChars()
        {
            _lowerChars = true;

            return this;
        }

        public RandomStringGenerator WithUpperChars()
        {
            _upperChars = true;

            return this;
        }

        public RandomStringGenerator WithNumbers()
        {
            _numbers = true;

            return this;
        }

        public string Generate()
        {
            var chars = "";

            StringBuilder result = new(_length);

            if (_lowerChars)
            {
                chars += LOWER_CHARS;
            }

            if (_upperChars)
            {
                chars += UPPER_CHARS;
            }

            if (_numbers)
            {
                chars += NUMBERS;
            }

            if (chars == "")
            {
                throw new NotAllowedException("No options provided");
            }

            char[] charPool = chars.ToCharArray();

            for (int i = 0; i < _length; i++)
            {
                var index = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, _length - 1, i - 1);

                result.Append(charPool[index]);
            }

            var generatedString = result.ToString();
            int? lastIndex = null;
            int randomIndex;

            if (_numbers && !RegexCollection.HasNumber().IsMatch(generatedString))
            {
                randomIndex = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, _length - 1, lastIndex);

                var numbersIndex = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, NUMBERS.Length);

                result[randomIndex] = NUMBERS.ToCharArray()[numbersIndex];

                lastIndex = randomIndex;
            }

            if (_lowerChars && !RegexCollection.HasLowerChar().IsMatch(generatedString))
            {
                randomIndex = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, _length - 1, lastIndex);

                var lowerCharIndex = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, LOWER_CHARS.Length);

                result[randomIndex] = LOWER_CHARS.ToCharArray()[lowerCharIndex];

                lastIndex = randomIndex;
            }

            if (_upperChars && !RegexCollection.HasUpperChar().IsMatch(generatedString))
            {
                randomIndex = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, _length - 1, lastIndex);

                var upperCharIndex = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, UPPER_CHARS.Length);

                result[randomIndex] = UPPER_CHARS.ToCharArray()[upperCharIndex];
            }

            var finalString = result.ToString();
            var matchesExcludedString = false;

            if (_excludedStrings.Length > 0)
            {
                matchesExcludedString = _excludedStrings.Any(excludedString => excludedString.Equals(finalString));
            }

            return matchesExcludedString ? Generate() : finalString;
        }
    }
}
