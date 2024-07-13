using FluentValidation;
using Microsoft.Extensions.Options;
using Moq;
using TechCraftsmen.User.Core.Configuration;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Interfaces.Services;
using TechCraftsmen.User.Core.Utils;
using TechCraftsmen.User.Core.Validation;
using TechCraftsmen.User.Tests.Utils.Generators;
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
        private readonly UserDtoGenerator _userDtoGenerator = new();
        private readonly Mock<IUserService> _userService;

        private const int TEST_ID = 1;
        private const string TEST_TOKEN_SECRET = "epnwawhzcxiykhuxzudhgioxwychqfjeaoebxinqmnhscgjwdz";
        private const string EXISTING_EMAIL = "exists@mail.com";
        private const string INEXISTING_EMAIL = "inexists@mail.com";
        private const string CORRECT_PASSWORD = "C0rrectPassw0rd";
        private const string INCORRECT_PASSWORD = "Inc0rrectPassw0rd";

        public AuthenticationServiceTests()
        {
            _authTokenConfig = new Mock<IOptions<AuthenticationTokenConfiguration>>();
            _authTokenConfigGenerator = new AuthenticationTokenConfigurationGenerator();
            _authTokenConfigValidator = new AuthenticationTokenConfigurationValidator();
            _authCredentialsGenerator = new AuthenticationCredentialsDtoGenerator();
            _authCredentialsValidator = new AuthenticationCredentialsDtoValidator();
            _userService = new Mock<IUserService>();

            _authTokenConfig.Setup(config => config.Value).Returns(_authTokenConfigGenerator.WithAudience("audience").WithIssuer("issuer").WithSeconds(60).WithSecret(TEST_TOKEN_SECRET).Generate());

            _userService.Setup(service => service.GetUsersByFilter(FilterUtils.CreateQuery("Email", EXISTING_EMAIL)))
                .Returns(() => [_userDtoGenerator.WithId(TEST_ID).WithEmail(EXISTING_EMAIL).WithPassword(CORRECT_PASSWORD).Generate()]);

            _userService.Setup(service => service.GetUsersByFilter(FilterUtils.CreateQuery("Email", INEXISTING_EMAIL)))
                .Returns(() => []);

            _userService.Setup(service => service.GetUserById(TEST_ID))
                .Returns(() => _userDtoGenerator.WithId(TEST_ID).WithEmail(EXISTING_EMAIL).WithPassword(CORRECT_PASSWORD).Generate());

            _userService.Setup(service => service.GetPasswordByUserId(TEST_ID))
                .Returns(() => HashUtils.HashText(CORRECT_PASSWORD));

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
            Assert.Throws<ValidationException>(() => _authenticationService.AuthenticateUser(_authCredentialsGenerator.WithEmail(string.Empty).WithPassword(string.Empty).Generate()));
        }

        [Fact]
        [Unit("AuthenticationService")]
        public void Should_ThrowNotAllowedException_ForInexistingEmail()
        {
            Assert.Throws<NotAllowedException>(() => _authenticationService.AuthenticateUser(_authCredentialsGenerator.WithEmail(INEXISTING_EMAIL).WithPassword(CORRECT_PASSWORD).Generate()));
        }

        [Fact]
        [Unit("AuthenticationService")]
        public void Should_ThrowNotAllowedException_ForIncorrectPassword()
        {
            Assert.Throws<NotAllowedException>(() => _authenticationService.AuthenticateUser(_authCredentialsGenerator.WithEmail(EXISTING_EMAIL).WithPassword(INCORRECT_PASSWORD).Generate()));
        }

        [Fact]
        [Unit("AuthenticationService")]
        public void Should_ThrowNotAllowedException_ForIncorrectCredentials()
        {
            Assert.Throws<NotAllowedException>(() => _authenticationService.AuthenticateUser(_authCredentialsGenerator.WithEmail(INEXISTING_EMAIL).WithPassword(INCORRECT_PASSWORD).Generate()));
        }

        [Fact]
        [Unit("AuthenticationService")]
        public void Should_ReturnIdFromToken()
        {
            var token = _authenticationService.AuthenticateUser(_authCredentialsGenerator.WithEmail(EXISTING_EMAIL).WithPassword(CORRECT_PASSWORD).Generate());
            var id = _authenticationService.GetUserIdFromJwtToken(token.AccessToken!);

            Assert.Equal(TEST_ID, id);
        }

        [Theory]
        [Unit("UserCreationRule")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_ReturnFalse_ForInvalidToken(string token)
        {
            var valid = _authenticationService.ValidateJwtToken(token, out var user);

            Assert.False(valid);
            Assert.Null(user);
        }

        [Fact]
        [Unit("AuthenticationService")]
        public void Should_GenerateValidToken_ForValidCredentials()
        {
            var token = _authenticationService.AuthenticateUser(_authCredentialsGenerator.WithEmail(EXISTING_EMAIL).WithPassword(CORRECT_PASSWORD).Generate());
            var valid = _authenticationService.ValidateJwtToken(token.AccessToken!, out var user);

            Assert.NotNull(token);
            Assert.True(valid);
            Assert.NotNull(user);
            Assert.True(user.Id == TEST_ID);
            Assert.True(user.Email == EXISTING_EMAIL);
        }
    }
}
