using TechCraftsmen.User.Domain.Enums;
using TechCraftsmen.User.Domain.Validation;
using TechCraftsmen.User.Utils.Security;

namespace TechCraftsmen.User.Tests.Mock.Generators
{
    public class UserGenerator
    {
        private readonly NameGenerator _nameGenerator = new();
        private static readonly EmailGenerator EmailGenerator = new();
        private static readonly RandomStringGenerator RandomStringGenerator = new();

        private int _id;
        private string _name = string.Empty;
        private string _email = string.Empty;
        private byte[] _password = [];
        private byte[] _salt = [];
        private int _roleId;
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
            _email = EmailGenerator.Generate();

            return this;
        }

        public UserGenerator WithPassword(string password)
        {
            Hash hash = new(password);

            _password = hash.Value;
            _salt = hash.Salt;

            return this;
        }

        public UserGenerator WithRandomPassword()
        {
            string randomString = RandomStringGenerator.WithLength(PasswordValidator.MINIMUM_LENGTH).WithLowerChars().WithUpperChars().WithNumbers().Generate();
            Hash hashResult = new(randomString);

            _password = hashResult.Value;
            _salt = hashResult.Salt;

            return this;
        }

        public UserGenerator WithRoleId(int roleId = (int)Roles.Test)
        {
            _roleId = roleId;

            return this;
        }

        public UserGenerator WithStatus(bool status)
        {
            _status = status;

            return this;
        }

        public Domain.Entities.User Generate()
        {
            Domain.Entities.User user = new()
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
