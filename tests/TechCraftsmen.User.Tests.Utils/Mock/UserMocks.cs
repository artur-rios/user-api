namespace TechCraftsmen.User.Tests.Utils.Mock
{

    public class UserMocks
    {
        public string TestPassword = "$k9!dTTW*zTe4uAHX";
        public int TEST_ID = 2;

        public Core.Entities.User TestUser = new()
        {
            Name = "Test User",
            Email = "test@mail.com",
            RoleId = 3
        };
    }
}
