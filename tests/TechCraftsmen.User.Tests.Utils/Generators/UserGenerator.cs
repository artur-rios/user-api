using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Rules.Password;
using TechCraftsmen.User.Core.Utils;

namespace TechCraftsmen.User.Tests.Utils.Generators
{
    public class UserGenerator
    {
        private readonly NameGenerator _nameGenerator = new();
        private static readonly EmailGenerator _emailGenerator = new();
        private static readonly RandomStringGenerator _randomStringGenerator = new();

        private int _id = 0;
        private string _name = string.Empty;
        private string _email = string.Empty;
        private byte[] _password = [];
        private byte[] _salt = [];
        private int _roleId = 0;
        private bool _status = true;

        public UserGenerator WithId(int id)
        {
            _id = id;

            return this;
        }

        public UserGenerator WithRandomId()
        {
            _id = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, 999);

            return this;
        }

        public UserGenerator WithName(string name)
        {
            _name = name;

            return this;
        }

        public UserGenerator WithDefaultName()
        {
            _name = _nameGenerator.Generate();

            return this;
        }

        public UserGenerator WithEmail(string email)
        {
            _email = email;

            return this;
        }

        public UserGenerator WithDefaultEmail()
        {
            _email = _emailGenerator.Generate();

            return this;
        }

        public UserGenerator WithPassword(string password)
        {
            var hashResult = HashUtils.HashText(password);

            _password = hashResult.Hash;
            _salt = hashResult.Salt;

            return this;
        }

        public UserGenerator WithRandomPassword()
        {
            var randomString = _randomStringGenerator.WithLength(PasswordRule.MINIMUM_LENGTH).WithLowerChars().WithUpperChars().WithNumbers().Generate();
            var hashResult = HashUtils.HashText(randomString);

            _password = hashResult.Hash;
            _salt = hashResult.Salt;

            return this;
        }

        public UserGenerator WithRoleId(int roleId = (int)Roles.TEST)
        {
            _roleId = roleId;

            return this;
        }

        public UserGenerator WithStatus(bool status)
        {
            _status = status;

            return this;
        }

        public Core.Entities.User Generate()
        {
            var user = new Core.Entities.User
            {
                Id = _id,
                Name = _name,
                Email = _email,
                Password = _password,
                Salt = _salt,
                RoleId = _roleId,
                Active = _status
            };

            return user;
        }
    }
}
