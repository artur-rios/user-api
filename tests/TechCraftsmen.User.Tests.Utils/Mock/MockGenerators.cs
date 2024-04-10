using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Rules.Password;

namespace TechCraftsmen.User.Tests.Utils.Mock
{
    public static class MockGenerators
    {
        public const string DEFAULT_USER_EMAIL = "jhon.doe@mail.com";
        public const string DEFAULT_USER_NAME = "Jhon Doe";

        private static readonly RandomStringGenerator _randomStringGenerator = new();

        public static Dictionary<string, object> Filter(string key, object value)
        {
            return new Dictionary<string, object>()
            {
                {key, value }
            };
        }

        public static string Name(int? id = null)
        {
            return id is null ? $"{DEFAULT_USER_NAME} {CustomRandomNumberGenerator.RandomStrongInt()}" : $"{DEFAULT_USER_NAME} {id}";
        }

        public static Core.Entities.User User()
        {
            var id = CustomRandomNumberGenerator.RandomStrongInt();

            return new Core.Entities.User()
            {
                Id = id,
                Name = Name(id)
            };
        }

        public static UserDto UserDto(Tuple<bool, string?>? nameData = null, Tuple<bool, string?>? emailData = null, Tuple<bool, string?>? passwordData = null)
        {
            var id = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, 999);

            bool enforceNullName = false;
            string? name = null;

            bool enforceNullEmail = false;
            string? email = null;

            bool enforceNullPassword = false;
            string? password = null;

            nameData?.Deconstruct(out enforceNullName, out name);
            emailData?.Deconstruct(out enforceNullEmail, out email);
            passwordData?.Deconstruct(out enforceNullPassword, out password);

#pragma warning disable CS8601 // Possible null reference assignment. Reason: expected null
            var user = new UserDto
            {
                Id = id,
                Name = name is not null || enforceNullName ? name : Name(id),
                Email = email is not null || enforceNullEmail ? email : DEFAULT_USER_EMAIL,
                Password = password is not null || enforceNullPassword ? password : _randomStringGenerator.WithLength(PasswordRule.MINIMUM_LENGTH).WithLowerChars().WithUpperChars().WithNumbers().Generate()
            };
#pragma warning restore CS8601 // Possible null reference assignment.

            return user;
        }
    }
}
