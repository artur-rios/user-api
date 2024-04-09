using TechCraftsmen.User.Core.Rules.Password;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Rules.Password
{
    public class PasswordRuleTests
    {
        private readonly PasswordRule _rule = new();

        [Fact]
        [Unit("PasswordRule")]
        public void Should_ReturnFalse_ForPassword_WithNoNumber()
        {
            var password = "Ab#cd#ef#";

            var result = _rule.Execute(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Password must contain a number", result.Errors.FirstOrDefault());
        }

        [Fact]
        [Unit("PasswordRule")]
        public void Should_ReturnFalse_ForPassword_WithNoLowerChar()
        {
            var password = "1A2B#CD#EF";

            var result = _rule.Execute(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Password must contain a lower char", result.Errors.FirstOrDefault());
        }

        [Fact]
        [Unit("PasswordRule")]
        public void Should_ReturnFalse_ForPassword_WithNoUpperChar()
        {
            var password = "1a2b#cd#ef";

            var result = _rule.Execute(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Password must contain a upper char", result.Errors.FirstOrDefault());
        }

        [Fact]
        [Unit("PasswordRule")]
        public void Should_ReturnFalse_ForPassword_WithLessThanMinimumLength()
        {
            var password = "1A2b#3c";

            var result = _rule.Execute(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal($"Password must contain at least {PasswordRule.MINIMUM_LENGTH} characters", result.Errors.FirstOrDefault());
        }

        [Theory]
        [Unit("PasswordRule")]
        [InlineData("")]
        [InlineData(null)]
        public void Should_ReturnFalse_ForPassword_NullOrEmpty(string password)
        {
            var result = _rule.Execute(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Parameter null or empty", result.Errors.FirstOrDefault());
        }

        [Fact]
        [Unit("PasswordRule")]
        public void Should_ReturnTrue_ForValidPassword()
        {
            var password = "1A2b#cd#ef";

            var result = _rule.Execute(password);

            Assert.True(result.Success);
            Assert.False(result.Errors.Any());
        }
    }
}
