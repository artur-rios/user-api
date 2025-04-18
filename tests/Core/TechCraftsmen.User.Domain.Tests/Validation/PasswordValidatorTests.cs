﻿using TechCraftsmen.User.Domain.Output;
using TechCraftsmen.User.Domain.Validation;
using TechCraftsmen.User.Tests.Configuration.Attributes;
using TechCraftsmen.User.Tests.Mock.Generators;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Validation
{
    public class PasswordValidatorTests
    {
        private readonly PasswordValidator _validator = new();
        private readonly RandomStringGenerator _randomStringGenerator = new();
        
        [UnitFact("PasswordValidator")]
        public void Should_ReturnFalse_ForPassword_WithNoNumber()
        {
            string password = _randomStringGenerator.WithLength(PasswordValidator.MINIMUM_LENGTH).WithLowerChars().WithUpperChars().Generate();

            ProcessOutput result = _validator.Validate(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Password must contain a number", result.Errors.FirstOrDefault());
        }
        
        [UnitFact("PasswordValidator")]
        public void Should_ReturnFalse_ForPassword_WithNoLowerChar()
        {
            string password = _randomStringGenerator.WithLength(PasswordValidator.MINIMUM_LENGTH).WithNumbers().WithUpperChars().Generate();

            ProcessOutput result = _validator.Validate(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Password must contain a lower char", result.Errors.FirstOrDefault());
        }
        
        [UnitFact("PasswordValidator")]
        public void Should_ReturnFalse_ForPassword_WithNoUpperChar()
        {
            string password = _randomStringGenerator.WithLength(PasswordValidator.MINIMUM_LENGTH).WithNumbers().WithLowerChars().Generate();

            ProcessOutput result = _validator.Validate(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Password must contain a upper char", result.Errors.FirstOrDefault());
        }

        [UnitFact("PasswordValidator")]
        public void Should_ReturnFalse_ForPassword_WithLessThanMinimumLength()
        {
            string password = _randomStringGenerator.WithLength(PasswordValidator.MINIMUM_LENGTH - 1).WithNumbers().WithLowerChars().WithUpperChars().Generate();

            ProcessOutput result = _validator.Validate(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal($"Password must contain at least {PasswordValidator.MINIMUM_LENGTH} characters", result.Errors.FirstOrDefault());
        }
        
        [UnitTheory("PasswordValidator")]
        [InlineData("")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_ReturnFalse_ForPassword_NullOrEmpty(string password)
        {
            ProcessOutput result = _validator.Validate(password);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Parameter null or empty", result.Errors.FirstOrDefault());
        }
        
        [UnitFact("PasswordValidator")]
        public void Should_ReturnTrue_ForValidPassword()
        {
            string password = _randomStringGenerator.WithLength(PasswordValidator.MINIMUM_LENGTH).WithNumbers().WithLowerChars().WithUpperChars().Generate();

            ProcessOutput result = _validator.Validate(password);

            Assert.True(result.Success);
            Assert.False(result.Errors.Any());
        }
    }
}
