using FluentValidation.TestHelper;
using TechCraftsmen.User.Core.Validation.Fluent;
using TechCraftsmen.User.Tests.Utils.Generators;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Validation.Fluent
{
    public class AuthenticationTokenConfigurationValidatorTests
    {
        private readonly AuthenticationTokenConfigurationGenerator _authTokenConfigGenerator = new();
        private readonly AuthenticationTokenConfigurationValidator _validator = new();

        [Theory]
        [Unit("AuthenticationTokenConfigurationValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_HaveError_ForAudienceNullOrEmpty(string? audience)
        {
            var authTokenConfigDto = _authTokenConfigGenerator.WithAudience(audience).WithIssuer("issuer").WithSeconds(60).WithSecret("secret").Generate();

            var result = _validator.TestValidate(authTokenConfigDto);

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
            var authTokenConfigDto = _authTokenConfigGenerator.WithAudience("audience").WithIssuer(issuer).WithSeconds(60).WithSecret("secret").Generate();

            var result = _validator.TestValidate(authTokenConfigDto);

            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Audience);
            result.ShouldHaveValidationErrorFor(authCredentials => authCredentials.Issuer);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Seconds);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Secret);
        }

        [Fact]
        [Unit("AuthenticationTokenConfigurationValidator")]
        public void Should_HaveError_ForZeroSeconds()
        {
            var authTokenConfigDto = _authTokenConfigGenerator.WithAudience("audience").WithIssuer("issuer").WithSeconds(0).WithSecret("secret").Generate();

            var result = _validator.TestValidate(authTokenConfigDto);

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
            var authTokenConfigDto = _authTokenConfigGenerator.WithAudience("audience").WithIssuer("issuer").WithSeconds(60).WithSecret(secret).Generate();

            var result = _validator.TestValidate(authTokenConfigDto);

            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Audience);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Issuer);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Seconds);
            result.ShouldHaveValidationErrorFor(authCredentials => authCredentials.Secret);
        }

        [Fact]
        [Unit("AuthenticationTokenConfigurationValidator")]
        public void Should_NotHaveError_ForValidAuthCredentials()
        {
            var authTokenConfigDto = _authTokenConfigGenerator.WithAudience("audience").WithIssuer("issuer").WithSeconds(60).WithSecret("secret").Generate();

            var result = _validator.TestValidate(authTokenConfigDto);

            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Audience);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Issuer);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Seconds);
            result.ShouldNotHaveValidationErrorFor(authCredentials => authCredentials.Secret);
        }
    }
}
