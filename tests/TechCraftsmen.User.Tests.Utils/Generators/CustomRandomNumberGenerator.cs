using System.Security.Cryptography;

namespace TechCraftsmen.User.Tests.Utils.Generators
{
    public static class CustomRandomNumberGenerator
    {
        public static int RandomStrongInt(bool allowNegatives = false, int startIndex = 0)
        {
            byte[] buffer = new byte[4];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }

            int random = BitConverter.ToInt32(buffer, startIndex);

            return allowNegatives ? random : random * -1;
        }

        public static int RandomStrongIntOnRange(int start, int end, int? differentFrom = null)
        {
            end++;

            int random = RandomNumberGenerator.GetInt32(start, end);

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
            int random = rng.Next(start, end);

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
