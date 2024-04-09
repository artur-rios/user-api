using TechCraftsmen.User.Core.Rules.User;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Rules.User
{
    public class UserDeletionRuleTests
    {
        private readonly UserDeletionRule _rule = new();

        [Fact]
        [Unit("UserDeletionRule")]
        public void Should_ReturnFalse_ForActiveUser()
        {
            var result = _rule.Execute(true);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Can't delete active user", result.Errors.FirstOrDefault());
        }

        [Fact]
        [Unit("UserDeletionRule")]
        public void Should_ReturnTrue_ForInactiveUser()
        {
            var result = _rule.Execute(false);

            Assert.True(result.Success);
            Assert.False(result.Errors.Any());
        }
    }
}
