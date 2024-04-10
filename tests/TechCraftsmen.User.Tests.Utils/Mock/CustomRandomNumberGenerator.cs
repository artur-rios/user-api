using System.Security.Cryptography;

namespace TechCraftsmen.User.Tests.Utils.Mock
{
    public static class CustomRandomNumberGenerator
    {
        public static int RandomStrongInt(bool allowNegatives = false, int startIndex = 0)
        {
            var buffer = new byte[4];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }

            var random = BitConverter.ToInt32(buffer, startIndex);

            return allowNegatives ? random : random * -1;
        }

        public static int RandomStrongIntOnRange(int start, int end, int? differentFrom = null)
        {
            end++;

            var random = RandomNumberGenerator.GetInt32(start, end);

            if (differentFrom is not null)
            {
                while (random == differentFrom)
                {
                    random = RandomNumberGenerator.GetInt32(start, end);
                }
            }

            return random;
        }

        public static int RandomWeakIntOnRange(int start, int end, int? differentFrom = null)
        {
            Random rng = new();
            var random = rng.Next(start, end);

            if (differentFrom is not null)
            {
                while (random == differentFrom)
                {
                    random = rng.Next(start, end);
                }
            }

            return random;
        }
    }
}
