using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace TechCraftsmen.User.Common.Utils
{
    public static class HashUtils
    {
        // No. of CPU Cores x 2
        private const int DEGREE_OF_PARALLELISM = 16;

        // Recommended minimum value
        private const int NUMBER_OF_ITERATIONS = 4;

        // 600 MB
        private const int MEMORY_TO_USE_IN_KB = 600000;

        public static HashResult HashText(string text, byte[]? salt = null)
        {
            salt ??= CreateSalt();

            var argon2id = new Argon2id(Encoding.UTF8.GetBytes(text))
            {
                Salt = salt,
                DegreeOfParallelism = DEGREE_OF_PARALLELISM,
                Iterations = NUMBER_OF_ITERATIONS,
                MemorySize = MEMORY_TO_USE_IN_KB
            };

            return new() { Hash = argon2id.GetBytes(128), Salt = salt };
        }

        public static bool VerifyHash(string text, byte[] salt, byte[] hash)
        {
            var result = HashText(text, salt);

            return hash.SequenceEqual(result.Hash);
        }

        public static byte[] CreateSalt()
        {
            var buffer = new byte[16];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }

            return buffer;
        }
    }

    public class HashResult
    {
        public byte[] Hash = [];
        public byte[] Salt = [];
    }
}
