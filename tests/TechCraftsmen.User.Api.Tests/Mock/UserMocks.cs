using Microsoft.Extensions.Diagnostics.HealthChecks;
using TechCraftsmen.User.Core.Entities;

namespace TechCraftsmen.User.Api.Tests;

public class UserMocks
{
    public string TestPassword = "$k9!dTTW*zTe4uAHX";

    public Core.Entities.User TestUser = new()
    {
        Name = "Test User",
        Email = "test@mail.com",
        RoleId = 3
    };
}
