using FluentValidation.TestHelper;
using TechCraftsmen.User.Core.Configuration;
using TechCraftsmen.User.Core.Validation;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Validation
{
    public class AuthenticationTokenConfigurationValidatorTests
    {
        private readonly AuthenticationTokenConfigurationValidator _validator = new();

        [Theory]
        [Unit("AuthenticationTokenConfigurationValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_HaveError_ForAudienceNullOrEmpty(string? audience)
        {
            var credentialsDto = new AuthenticationTokenConfiguration() { Audience = audience, Issuer = "issuer", Seconds = 60, Secret = "secret" };

            var result = _validator.TestValidate(credentialsDto);

            result.ShouldHaveValidationErrorFor(authCredentials => authCredentials.Audience);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Issuer);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Seconds);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Secret);
        }

        [Theory]
        [Unit("AuthenticationTokenConfigurationValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_HaveError_ForIssuerNullOrEmpty(string? issuer)
        {
            var credentialsDto = new AuthenticationTokenConfiguration() { Audience = "audience", Issuer = issuer, Seconds = 60, Secret = "secret" };

            var result = _validator.TestValidate(credentialsDto);

            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Audience);
            result.ShouldHaveValidationErrorFor(authCredentials => authCredentials.Issuer);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Seconds);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Secret);
        }

        [Fact]
        [Unit("AuthenticationTokenConfigurationValidator")]
        public void Should_HaveError_ForZeroSeconds()
        {
            var credentialsDto = new AuthenticationTokenConfiguration() { Audience = "audience", Issuer = "issuer", Seconds = 0, Secret = "secret" };

            var result = _validator.TestValidate(credentialsDto);

            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Audience);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Issuer);
            result.ShouldHaveValidationErrorFor(authCredentials => authCredentials.Seconds);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Secret);
        }

        [Theory]
        [Unit("AuthenticationTokenConfigurationValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_HaveError_ForSecretNullOrEmpty(string? secret)
        {
            var credentialsDto = new AuthenticationTokenConfiguration() { Audience = "audience", Issuer = "issuer", Seconds = 60, Secret = secret };

            var result = _validator.TestValidate(credentialsDto);

            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Audience);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Issuer);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Seconds);
            result.ShouldHaveValidationErrorFor(authCredentials => authCredentials.Secret);
        }

        [Fact]
        [Unit("AuthenticationTokenConfigurationValidator")]
        public void Should_NotHaveError_ForValidAuthCredentials()
        {
            var credentialsDto = new AuthenticationTokenConfiguration() { Audience = "audience", Issuer = "issuer", Seconds = 60, Secret = "secret" };

            var result = _validator.TestValidate(credentialsDto);

            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Audience);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Issuer);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Seconds);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Secret);
        }
    }
}
