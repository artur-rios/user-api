using FluentValidation.TestHelper;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Validation.Fluent;
using TechCraftsmen.User.Tests.Utils.Generators;
using TechCraftsmen.User.Tests.Utils.Attributes;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Validation.Fluent
{
    public class AuthenticationCredentialsDtoValidatorTests
    {
        private readonly AuthenticationCredentialsDtoGenerator _authCredentialsGenerator = new();
        private readonly AuthenticationCredentialsDtoValidator _validator = new();
        
        [UnitTheory("AuthenticationCredentialsDtoValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_HaveError_ForEmailNullOrEmpty(string email)
        {
            AuthenticationCredentialsDto credentialsDto = _authCredentialsGenerator.WithEmail(email).WithRandomPassword().Generate();

            TestValidationResult<AuthenticationCredentialsDto>? result = _validator.TestValidate(credentialsDto);

            result.ShouldHaveValidationErrorFor(authCredentials => authCredentials.Email);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Password);

        }
        
        [UnitTheory("AuthenticationCredentialsDtoValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_HaveError_ForPasswordNullOrEmpty(string password)
        {
            AuthenticationCredentialsDto credentialsDto = _authCredentialsGenerator.WithDefaultEmail().WithPassword(password).Generate();

            TestValidationResult<AuthenticationCredentialsDto>? result = _validator.TestValidate(credentialsDto);

            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Email);
            result.ShouldHaveValidationErrorFor(authCredentials => authCredentials.Password);
        }
        
        [UnitTheory("AuthenticationCredentialsDtoValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_HaveError_ForPasswordAndEmailNullOrEmpty(string invalidString)
        {
            AuthenticationCredentialsDto credentialsDto = _authCredentialsGenerator.WithEmail(invalidString).WithPassword(invalidString).Generate();

            TestValidationResult<AuthenticationCredentialsDto>? result = _validator.TestValidate(credentialsDto);

            result.ShouldHaveValidationErrorFor(authCredentials => authCredentials.Email);
            result.ShouldHaveValidationErrorFor(authCredentials => authCredentials.Password);
        }
        
        [UnitFact("AuthenticationCredentialsDtoValidator")]
        public void Should_NotHaveError_ForValidAuthCredentials()
        {
            AuthenticationCredentialsDto credentialsDto = _authCredentialsGenerator.WithDefaultEmail().WithRandomPassword().Generate();

            TestValidationResult<AuthenticationCredentialsDto>? result = _validator.TestValidate(credentialsDto);

            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Email);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Password);
        }
    }
}
