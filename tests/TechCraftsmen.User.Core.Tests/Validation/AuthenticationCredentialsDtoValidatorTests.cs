using FluentValidation.TestHelper;
using TechCraftsmen.User.Core.Validation;
using TechCraftsmen.User.Tests.Utils.Mock;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Validation
{
    public class AuthenticationCredentialsDtoValidatorTests
    {
        private readonly AuthenticationCredentialsDtoGenerator _authCredentialsGenerator = new();
        private readonly AuthenticationCredentialsDtoValidator _validator = new();

        [Theory]
        [Unit("AuthenticationCredentialsDtoValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_HaveError_ForEmailNullOrEmpty(string email)
        {
            var credentialsDto = _authCredentialsGenerator.WithEmail(email).WithRandomPassword().Generate();

            var result = _validator.TestValidate(credentialsDto);

            result.ShouldHaveValidationErrorFor(authCredentials => authCredentials.Email);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Password);

        }

        [Theory]
        [Unit("AuthenticationCredentialsDtoValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_HaveError_ForPasswordNullOrEmpty(string password)
        {
            var credentialsDto = _authCredentialsGenerator.WithDefaultEmail().WithPassword(password).Generate();

            var result = _validator.TestValidate(credentialsDto);

            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Email);
            result.ShouldHaveValidationErrorFor(authCredentials => authCredentials.Password);
        }

        [Theory]
        [Unit("AuthenticationCredentialsDtoValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_HaveError_ForPasswordAndEmailNullOrEmpty(string invalidString)
        {
            var credentialsDto = _authCredentialsGenerator.WithEmail(invalidString).WithPassword(invalidString).Generate();

            var result = _validator.TestValidate(credentialsDto);

            result.ShouldHaveValidationErrorFor(authCredentials => authCredentials.Email);
            result.ShouldHaveValidationErrorFor(authCredentials => authCredentials.Password);
        }

        [Fact]
        [Unit("AuthenticationCredentialsDtoValidator")]
        public void Should_NotHaveError_ForValidAuthCredentials()
        {
            var credentialsDto = _authCredentialsGenerator.WithDefaultEmail().WithRandomPassword().Generate();

            var result = _validator.TestValidate(credentialsDto);

            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Email);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Password);
        }
    }
}
