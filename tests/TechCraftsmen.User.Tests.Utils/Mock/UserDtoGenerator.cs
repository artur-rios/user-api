using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Rules.Password;

namespace TechCraftsmen.User.Tests.Utils.Mock
{
    public class UserDtoGenerator
    {
        private static readonly NameGenerator _nameGenerator = new();
        private static readonly EmailGenerator _emailGenerator = new();
        private static readonly RandomStringGenerator _randomStringGenerator = new();

        private int _id = 0;
        private string? _name = null;
        private string? _email = null;
        private string? _password = null;

        public UserDtoGenerator WithId(int id)
        {
            _id = id;

            return this;
        }

        public UserDtoGenerator WithRandomId()
        {
            _id = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, 999);

            return this;
        }

        public UserDtoGenerator WithName(string name)
        {
            _name = name;

            return this;
        }

        public UserDtoGenerator WithDefaultName()
        {
            _name = _nameGenerator.Generate();

            return this;
        }

        public UserDtoGenerator WithEmail(string email)
        {
            _email = email;

            return this;
        }

        public UserDtoGenerator WithDefaultEmail()
        {
            _email = _emailGenerator.Generate();

            return this;
        }

        public UserDtoGenerator WithPassword(string password)
        {
            _password = password;

            return this;
        }

        public UserDtoGenerator WithRandomPassword()
        {
            _password = _randomStringGenerator.WithLength(PasswordRule.MINIMUM_LENGTH).WithLowerChars().WithUpperChars().WithNumbers().Generate();

            return this;
        }

        public UserDto Generate()
        {
#pragma warning disable CS8601 // Possible null reference assignment. Reason: null expected
            var user = new UserDto
            {
                Id = _id,
                Name = _name,
                Email = _email,
                Password = _password
            };
#pragma warning restore CS8601 // Possible null reference assignment.

            return user;
        }
    }
}
