using FluentValidation;
using Microsoft.Extensions.Options;
using Moq;
using TechCraftsmen.User.Core.Configuration;
using TechCraftsmen.User.Core.Interfaces.Services;
using TechCraftsmen.User.Core.Validation;
using TechCraftsmen.User.Tests.Utils.Mock;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Services.Tests
{
    public class AuthenticationServiceTests
    {
        private readonly AuthenticationService _authenticationService;
        private readonly Mock<IOptions<AuthenticationTokenConfiguration>> _authTokenConfig;
        private readonly AuthenticationTokenConfigurationGenerator _authTokenConfigGenerator;
        private readonly AuthenticationTokenConfigurationValidator _authTokenConfigValidator;
        private readonly AuthenticationCredentialsDtoGenerator _authCredentialsGenerator;
        private readonly AuthenticationCredentialsDtoValidator _authCredentialsValidator;
        private readonly Mock<IUserService> _userService;

        public AuthenticationServiceTests()
        {
            _authTokenConfig = new Mock<IOptions<AuthenticationTokenConfiguration>>();
            _authTokenConfigGenerator = new AuthenticationTokenConfigurationGenerator();
            _authTokenConfigValidator = new AuthenticationTokenConfigurationValidator();
            _authCredentialsGenerator = new AuthenticationCredentialsDtoGenerator();
            _authCredentialsValidator = new AuthenticationCredentialsDtoValidator();
            _userService = new Mock<IUserService>();

            _authTokenConfig.Setup(config => config.Value).Returns(_authTokenConfigGenerator.WithAudience("audience").WithIssuer("issuer").WithSeconds(60).WithSecret("secret").Generate());

            _authenticationService = new AuthenticationService(_authTokenConfig.Object, _authCredentialsValidator, _authTokenConfigValidator, _userService.Object);
        }

        [Fact]
        [Unit("AuthenticationService")]
        public void Should_ThrowValidationException_IfAuthTokenConfigIsMissing()
        {
            _authTokenConfig.Setup(config => config.Value).Returns(_authTokenConfigGenerator.WithAudience("audience").WithIssuer("issuer").WithSeconds(0).WithSecret("").Generate());

            Assert.Throws<ValidationException>(() => new AuthenticationService(_authTokenConfig.Object, _authCredentialsValidator, _authTokenConfigValidator, _userService.Object));
        }

        [Fact]
        [Unit("AuthenticationService")]
        public void Should_ThrowValidationException_ForInvalidCredentials()
        {
            Assert.Throws<ValidationException>(() => _authenticationService.AuthenticateUser(_authCredentialsGenerator.WithEmail("").WithPassword("").Generate()));
        }
    }
}
