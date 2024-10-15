using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;

namespace TechCraftsmen.User.Tests.Mock.Data
{
    public class UserMockData
    {
        public const string TEST_PASSWORD = "$k9!dTTW*zTe4uAHX";
        public const int TEST_ID = 2;

        public readonly Core.Entities.User TestUser = new()
        {
            Id = TEST_ID,
            Name = "Test User",
            Email = "test@mail.com",
            RoleId = (int)Roles.Test
        };

        public readonly UserDto TestUserDto = new()
        {
            Id = TEST_ID,
            Name = "Test User",
            Email = "test@mail.com",
            RoleId = (int)Roles.Test
        };
    }
}
