using System.Security.Cryptography;

namespace TechCraftsmen.User.Tests.Utils.Mock
{
    public static class MockGenerators
    {
        public static Dictionary<string, object> GenerateFilter(string key, object value)
        {
            return new Dictionary<string, object>()
            {
                {key, value }
            };
        }

        public static string GenerateName(int? id = null)
        {
            return id is null ? $"Jhon Doe {GenerateRandomNumber()}" : $"Jhon Doe {id}";
        }

        public static int GenerateRandomNumber()
        {
            var buffer = new byte[4];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }

            return BitConverter.ToInt32(buffer, 0);
        }

        public static Core.Entities.User GenerateUser()
        {
            var id = GenerateRandomNumber();

            return new Core.Entities.User()
            {
                Id = id,
                Name = GenerateName(id),
                RoleId = 2
            };
        }
    }
}
