using System.Text;
using TechCraftsmen.User.Core.Collections;
using TechCraftsmen.User.Core.Exceptions;

namespace TechCraftsmen.User.Tests.Utils.Mock
{
    public class RandomStringGenerator
    {
        private const string LOWER_CHARS = "abcdefghijklmnopqrstuvwxyz";
        private const string UPPER_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string NUMBERS = "0123456789";

        private int Length;
        private bool LowerChars;
        private bool UpperChars;
        private bool Numbers;

        public RandomStringGenerator WithLength(int length)
        {
            Length = length;

            return this;
        }

        public RandomStringGenerator WithLowerChars()
        {
            LowerChars = true;

            return this;
        }

        public RandomStringGenerator WithUpperChars()
        {
            UpperChars = true;

            return this;
        }

        public RandomStringGenerator WithNumbers()
        {
            Numbers = true;

            return this;
        }

        public string Generate()
        {
            var chars = "";

            StringBuilder result = new(Length);

            if (LowerChars)
            {
                chars += LOWER_CHARS;
            }

            if (UpperChars)
            {
                chars += UPPER_CHARS;
            }

            if (Numbers)
            {
                chars += NUMBERS;
            }

            if (chars == "")
            {
                throw new NotAllowedException("No options provided");
            }

            char[] charPool = chars.ToCharArray();

            for (int i = 0; i < Length; i++)
            {
                var index = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, Length - 1, i - 1);

                result.Append(charPool[index]);
            }

            var generatedString = result.ToString();
            int? lastIndex = null;
            int randomIndex;

            if (Numbers && !RegexCollection.HasNumber().IsMatch(generatedString))
            {
                randomIndex = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, Length - 1, lastIndex);

                var numbersIndex = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, NUMBERS.Length);

                result[randomIndex] = NUMBERS.ToCharArray()[numbersIndex];

                lastIndex = randomIndex;
            }

            if (LowerChars && !RegexCollection.HasLowerChar().IsMatch(generatedString))
            {
                randomIndex = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, Length - 1, lastIndex);

                var lowerCharIndex = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, LOWER_CHARS.Length);

                result[randomIndex] = LOWER_CHARS.ToCharArray()[lowerCharIndex];

                lastIndex = randomIndex;
            }

            if (UpperChars && !RegexCollection.HasUpperChar().IsMatch(generatedString))
            {
                randomIndex = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, Length - 1, lastIndex);

                var upperCharIndex = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, UPPER_CHARS.Length);

                result[randomIndex] = UPPER_CHARS.ToCharArray()[upperCharIndex];
            }

            return result.ToString();
        }
    }
}
