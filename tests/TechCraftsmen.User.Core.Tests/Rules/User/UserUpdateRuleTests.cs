using TechCraftsmen.User.Core.Rules.User;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Rules.User
{
    public class UserUpdateRuleTests
    {
        private readonly UserUpdateRule _rule = new();

        [Fact]
        [Unit("UserUpdateRule")]
        public void Should_ReturnFalse_ForInactiveUser()
        {
            var result = _rule.Execute(false);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Can't update inactive user", result.Errors.FirstOrDefault());
        }

        [Fact]
        [Unit("UserUpdateRule")]
        public void Should_ReturnTrue_ForAactiveUser()
        {
            var result = _rule.Execute(true);

            Assert.True(result.Success);
            Assert.False(result.Errors.Any());
        }
    }
}
