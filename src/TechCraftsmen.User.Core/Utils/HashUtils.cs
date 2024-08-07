using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Core.Utils
{
    public static class HashUtils
    {
        // No. of CPU Cores x 2
        private const int DegreeOfParallelism = 16;

        // Recommended minimum value
        private const int NumberOfIterations = 4;

        // 600 MB
        private const int MemoryToUseInKb = 600000;

        public static HashDto HashText(string text, byte[]? salt = null)
        {
            salt ??= CreateSalt();

            Argon2id argon2Id = new Argon2id(Encoding.UTF8.GetBytes(text))
            {
                Salt = salt,
                DegreeOfParallelism = DegreeOfParallelism,
                Iterations = NumberOfIterations,
                MemorySize = MemoryToUseInKb
            };

            return new() { Hash = argon2Id.GetBytes(128), Salt = salt };
        }

        public static bool VerifyHash(string text, HashDto hashDto)
        {
            HashDto result = HashText(text, hashDto.Salt);

            return hashDto.Hash.SequenceEqual(result.Hash);
        }

        public static byte[] CreateSalt()
        {
            byte[] buffer = new byte[16];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }

            return buffer;
        }
    }
}
