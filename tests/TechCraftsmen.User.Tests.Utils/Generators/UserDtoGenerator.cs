using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Validation;

namespace TechCraftsmen.User.Tests.Utils.Generators
{
    public class UserDtoGenerator
    {
        private static readonly NameGenerator _nameGenerator = new();
        private static readonly EmailGenerator _emailGenerator = new();
        private static readonly RandomStringGenerator _randomStringGenerator = new();

        private int _id = 0;
        private string _name = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private int _roleId = 0;

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
            _password = _randomStringGenerator.WithLength(PasswordValidator.MINIMUM_LENGTH).WithLowerChars().WithUpperChars().WithNumbers().Generate();

            return this;
        }

        public UserDtoGenerator WithRoleId(int roleId = (int)Roles.TEST)
        {
            _roleId = roleId;

            return this;
        }

        public UserDto Generate()
        {
            var user = new UserDto
            {
                Id = _id,
                Name = _name,
                Email = _email,
                Password = _password,
                RoleId = _roleId
            };

            return user;
        }
    }
}
