using FluentValidation.TestHelper;
using TechCraftsmen.User.Core.Configuration;
using TechCraftsmen.User.Core.Validation.Fluent;
using TechCraftsmen.User.Tests.Configuration.Attributes;
using TechCraftsmen.User.Tests.Mock.Generators;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Validation.Fluent
{
    public class AuthenticationTokenConfigurationValidatorTests
    {
        private readonly AuthenticationTokenConfigurationGenerator _authTokenConfigGenerator = new();
        private readonly AuthenticationTokenConfigurationValidator _validator = new();
        
        [UnitTheory("AuthenticationTokenConfigurationValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_HaveError_ForAudienceNullOrEmpty(string? audience)
        {
            AuthenticationTokenConfiguration authTokenConfigDto = _authTokenConfigGenerator.WithAudience(audience).WithIssuer("issuer").WithSeconds(60).WithSecret("secret").Generate();

            TestValidationResult<AuthenticationTokenConfiguration>? result = _validator.TestValidate(authTokenConfigDto);

            result.ShouldHaveValidationErrorFor(authCredentials => authCredentials.Audience);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Issuer);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Seconds);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Secret);
        }
        
        [UnitTheory("AuthenticationTokenConfigurationValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_HaveError_ForIssuerNullOrEmpty(string? issuer)
        {
            AuthenticationTokenConfiguration authTokenConfigDto = _authTokenConfigGenerator.WithAudience("audience").WithIssuer(issuer).WithSeconds(60).WithSecret("secret").Generate();

            TestValidationResult<AuthenticationTokenConfiguration>? result = _validator.TestValidate(authTokenConfigDto);

            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Audience);
            result.ShouldHaveValidationErrorFor(authCredentials => authCredentials.Issuer);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Seconds);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Secret);
        }

        [UnitFact("AuthenticationTokenConfigurationValidator")]
        public void Should_HaveError_ForZeroSeconds()
        {
            AuthenticationTokenConfiguration authTokenConfigDto = _authTokenConfigGenerator.WithAudience("audience").WithIssuer("issuer").WithSeconds(0).WithSecret("secret").Generate();

            TestValidationResult<AuthenticationTokenConfiguration>? result = _validator.TestValidate(authTokenConfigDto);

            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Audience);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Issuer);
            result.ShouldHaveValidationErrorFor(authCredentials => authCredentials.Seconds);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Secret);
        }
        
        [UnitTheory("AuthenticationTokenConfigurationValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_HaveError_ForSecretNullOrEmpty(string? secret)
        {
            AuthenticationTokenConfiguration authTokenConfigDto = _authTokenConfigGenerator.WithAudience("audience").WithIssuer("issuer").WithSeconds(60).WithSecret(secret).Generate();

            TestValidationResult<AuthenticationTokenConfiguration>? result = _validator.TestValidate(authTokenConfigDto);

            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Audience);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Issuer);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Seconds);
            result.ShouldHaveValidationErrorFor(authCredentials => authCredentials.Secret);
        }
        
        [UnitFact("AuthenticationTokenConfigurationValidator")]
        public void Should_NotHaveError_ForValidAuthCredentials()
        {
            AuthenticationTokenConfiguration authTokenConfigDto = _authTokenConfigGenerator.WithAudience("audience").WithIssuer("issuer").WithSeconds(60).WithSecret("secret").Generate();

            TestValidationResult<AuthenticationTokenConfiguration>? result = _validator.TestValidate(authTokenConfigDto);

            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Audience);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Issuer);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Seconds);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Secret);
        }
    }
}
