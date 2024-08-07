using System.Text;
using TechCraftsmen.User.Core.Collections;
using TechCraftsmen.User.Core.Exceptions;

namespace TechCraftsmen.User.Tests.Utils.Generators
{
    public class RandomStringGenerator
    {
        private const string LowerChars = "abcdefghijklmnopqrstuvwxyz";
        private const string UpperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Numbers = "0123456789";

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
            string chars = "";

            StringBuilder result = new(_length);

            if (_lowerChars)
            {
                chars += LowerChars;
            }

            if (_upperChars)
            {
                chars += UpperChars;
            }

            if (_numbers)
            {
                chars += Numbers;
            }

            if (chars == "")
            {
                throw new NotAllowedException("No options provided");
            }

            char[] charPool = chars.ToCharArray();

            for (int i = 0; i < _length; i++)
            {
                int index = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, _length - 1, i - 1);

                result.Append(charPool[index]);
            }

            string generatedString = result.ToString();
            int? lastIndex = null;
            int randomIndex;

            if (_numbers && !RegexCollection.HasNumber().IsMatch(generatedString))
            {
                randomIndex = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, _length - 1, lastIndex);

                int numbersIndex = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, Numbers.Length);

                result[randomIndex] = Numbers.ToCharArray()[numbersIndex];

                lastIndex = randomIndex;
            }

            if (_lowerChars && !RegexCollection.HasLowerChar().IsMatch(generatedString))
            {
                randomIndex = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, _length - 1, lastIndex);

                int lowerCharIndex = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, LowerChars.Length);

                result[randomIndex] = LowerChars.ToCharArray()[lowerCharIndex];

                lastIndex = randomIndex;
            }

            if (_upperChars && !RegexCollection.HasUpperChar().IsMatch(generatedString))
            {
                randomIndex = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, _length - 1, lastIndex);

                int upperCharIndex = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, UpperChars.Length);

                result[randomIndex] = UpperChars.ToCharArray()[upperCharIndex];
            }

            string finalString = result.ToString();
            bool matchesExcludedString = false;

            if (_excludedStrings.Length > 0)
            {
                matchesExcludedString = _excludedStrings.Any(excludedString => excludedString.Equals(finalString));
            }

            return matchesExcludedString ? Generate() : finalString;
        }
    }
}
