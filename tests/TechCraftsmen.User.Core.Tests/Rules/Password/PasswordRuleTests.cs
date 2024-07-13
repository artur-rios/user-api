using TechCraftsmen.User.Core.Rules.Password;
using TechCraftsmen.User.Tests.Utils.Generators;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Rules.Password
{
    public class PasswordRuleTests
    {
        private readonly PasswordRule _rule = new();
        private readonly RandomStringGenerator _randomStringGenerator = new();

        [Fact]
        [Unit("PasswordRule")]
        public void Should_ReturnFalse_ForPassword_WithNoNumber()
        {
            var password = _randomStringGenerator.WithLength(PasswordRule.MINIMUM_LENGTH).WithLowerChars().WithUpperChars().Generate();

            var result = _rule.Execute(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Password must contain a number", result.Errors.FirstOrDefault());
        }

        [Fact]
        [Unit("PasswordRule")]
        public void Should_ReturnFalse_ForPassword_WithNoLowerChar()
        {
            var password = _randomStringGenerator.WithLength(PasswordRule.MINIMUM_LENGTH).WithNumbers().WithUpperChars().Generate();

            var result = _rule.Execute(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Password must contain a lower char", result.Errors.FirstOrDefault());
        }

        [Fact]
        [Unit("PasswordRule")]
        public void Should_ReturnFalse_ForPassword_WithNoUpperChar()
        {
            var password = _randomStringGenerator.WithLength(PasswordRule.MINIMUM_LENGTH).WithNumbers().WithLowerChars().Generate();

            var result = _rule.Execute(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Password must contain a upper char", result.Errors.FirstOrDefault());
        }

        [Fact]
        [Unit("PasswordRule")]
        public void Should_ReturnFalse_ForPassword_WithLessThanMinimumLength()
        {
            var password = _randomStringGenerator.WithLength(PasswordRule.MINIMUM_LENGTH - 1).WithNumbers().WithLowerChars().WithUpperChars().Generate();

            var result = _rule.Execute(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal($"Password must contain at least {PasswordRule.MINIMUM_LENGTH} characters", result.Errors.FirstOrDefault());
        }

        [Theory]
        [Unit("PasswordRule")]
        [InlineData("")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
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
            var password = _randomStringGenerator.WithLength(PasswordRule.MINIMUM_LENGTH).WithNumbers().WithLowerChars().WithUpperChars().Generate();

            var result = _rule.Execute(password);

            Assert.True(result.Success);
            Assert.False(result.Errors.Any());
        }
    }
}
