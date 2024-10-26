using System.Security.Cryptography;

namespace TechCraftsmen.User.Tests.Mock.Generators
{
    public static class CustomRandomNumberGenerator
    {
        public static int RandomWeakIntOnRange(int start, int end, int? differentFrom = null)
        {
            Random rng = new();
            
            int random = rng.Next(start, end);

            if (differentFrom is null)
            {
                return random;
            }

            while (random == differentFrom)
            {
                random = rng.Next(start, end);
            }

            return random;
        }
        
        public static byte[] RandomNumberBytes()
        {
            byte[] buffer = new byte[16];

            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(buffer);

            return buffer;
        }
    }
}
