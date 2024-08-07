using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Utils;
using TechCraftsmen.User.Core.Validation;

namespace TechCraftsmen.User.Tests.Utils.Generators
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
            HashDto hashResult = HashUtils.HashText(password);

            _password = hashResult.Hash;
            _salt = hashResult.Salt;

            return this;
        }

        public UserGenerator WithRandomPassword()
        {
            string randomString = RandomStringGenerator.WithLength(PasswordValidator.MINIMUM_LENGTH).WithLowerChars().WithUpperChars().WithNumbers().Generate();
            HashDto hashResult = HashUtils.HashText(randomString);

            _password = hashResult.Hash;
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

        public Core.Entities.User Generate()
        {
            Core.Entities.User user = new()
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
