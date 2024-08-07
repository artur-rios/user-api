using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Validation;
using TechCraftsmen.User.Tests.Utils.Generators;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Validation
{
    public class PasswordValidatorTests
    {
        private readonly PasswordValidator _validator = new();
        private readonly RandomStringGenerator _randomStringGenerator = new();

        [Fact]
        [Unit("PasswordValidator")]
        public void Should_ReturnFalse_ForPassword_WithNoNumber()
        {
            string password = _randomStringGenerator.WithLength(PasswordValidator.MINIMUM_LENGTH).WithLowerChars().WithUpperChars().Generate();

            SimpleResultDto result = _validator.Validate(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Password must contain a number", result.Errors.FirstOrDefault());
        }

        [Fact]
        [Unit("PasswordValidator")]
        public void Should_ReturnFalse_ForPassword_WithNoLowerChar()
        {
            string password = _randomStringGenerator.WithLength(PasswordValidator.MINIMUM_LENGTH).WithNumbers().WithUpperChars().Generate();

            SimpleResultDto result = _validator.Validate(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Password must contain a lower char", result.Errors.FirstOrDefault());
        }

        [Fact]
        [Unit("PasswordValidator")]
        public void Should_ReturnFalse_ForPassword_WithNoUpperChar()
        {
            string password = _randomStringGenerator.WithLength(PasswordValidator.MINIMUM_LENGTH).WithNumbers().WithLowerChars().Generate();

            SimpleResultDto result = _validator.Validate(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Password must contain a upper char", result.Errors.FirstOrDefault());
        }

        [Fact]
        [Unit("PasswordValidator")]
        public void Should_ReturnFalse_ForPassword_WithLessThanMinimumLength()
        {
            string password = _randomStringGenerator.WithLength(PasswordValidator.MINIMUM_LENGTH - 1).WithNumbers().WithLowerChars().WithUpperChars().Generate();

            SimpleResultDto result = _validator.Validate(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal($"Password must contain at least {PasswordValidator.MINIMUM_LENGTH} characters", result.Errors.FirstOrDefault());
        }

        [Theory]
        [Unit("PasswordValidator")]
        [InlineData("")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_ReturnFalse_ForPassword_NullOrEmpty(string password)
        {
            SimpleResultDto result = _validator.Validate(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Parameter null or empty", result.Errors.FirstOrDefault());
        }

        [Fact]
        [Unit("PasswordValidator")]
        public void Should_ReturnTrue_ForValidPassword()
        {
            string password = _randomStringGenerator.WithLength(PasswordValidator.MINIMUM_LENGTH).WithNumbers().WithLowerChars().WithUpperChars().Generate();

            SimpleResultDto result = _validator.Validate(password);

            Assert.True(result.Success);
            Assert.False(result.Errors.Any());
        }
    }
}
