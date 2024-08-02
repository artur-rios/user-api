using FluentValidation;
using Microsoft.Extensions.Options;
using Moq;
using TechCraftsmen.User.Core.Configuration;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Interfaces.Services;
using TechCraftsmen.User.Core.Utils;
using TechCraftsmen.User.Core.Validation.Fluent;
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

        private const int TestId = 1;
        private const string TestTokenSecret = "epnwawhzcxiykhuxzudhgioxwychqfjeaoebxinqmnhscgjwdz";
        private const string ExistingEmail = "exists@mail.com";
        private const string NonexistentEmail = "inexists@mail.com";
        private const string CorrectPassword = "C0rrectPassw0rd";
        private const string IncorrectPassword = "Inc0rrectPassw0rd";

        public AuthenticationServiceTests()
        {
            _authTokenConfig = new Mock<IOptions<AuthenticationTokenConfiguration>>();
            _authTokenConfigGenerator = new AuthenticationTokenConfigurationGenerator();
            _authTokenConfigValidator = new AuthenticationTokenConfigurationValidator();
            _authCredentialsGenerator = new AuthenticationCredentialsDtoGenerator();
            _authCredentialsValidator = new AuthenticationCredentialsDtoValidator();
            _userService = new Mock<IUserService>();

            _authTokenConfig.Setup(config => config.Value).Returns(_authTokenConfigGenerator.WithAudience("audience").WithIssuer("issuer").WithSeconds(60).WithSecret(TestTokenSecret).Generate());

            _userService.Setup(service => service.GetUsersByFilter(FilterUtils.CreateQuery("Email", ExistingEmail)))
                .Returns(() => [_userDtoGenerator.WithId(TestId).WithEmail(ExistingEmail).WithPassword(CorrectPassword).Generate()]);

            _userService.Setup(service => service.GetUsersByFilter(FilterUtils.CreateQuery("Email", NonexistentEmail)))
                .Returns(() => []);

            _userService.Setup(service => service.GetUserById(TestId))
                .Returns(() => _userDtoGenerator.WithId(TestId).WithEmail(ExistingEmail).WithPassword(CorrectPassword).Generate());

            _userService.Setup(service => service.GetPasswordByUserId(TestId))
                .Returns(() => HashUtils.HashText(CorrectPassword));

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
        public void Should_ThrowNotAllowedException_ForNonexistentEmail()
        {
            Assert.Throws<NotAllowedException>(() => _authenticationService.AuthenticateUser(_authCredentialsGenerator.WithEmail(NonexistentEmail).WithPassword(CorrectPassword).Generate()));
        }

        [Fact]
        [Unit("AuthenticationService")]
        public void Should_ThrowNotAllowedException_ForIncorrectPassword()
        {
            Assert.Throws<NotAllowedException>(() => _authenticationService.AuthenticateUser(_authCredentialsGenerator.WithEmail(ExistingEmail).WithPassword(IncorrectPassword).Generate()));
        }

        [Fact]
        [Unit("AuthenticationService")]
        public void Should_ThrowNotAllowedException_ForIncorrectCredentials()
        {
            Assert.Throws<NotAllowedException>(() => _authenticationService.AuthenticateUser(_authCredentialsGenerator.WithEmail(NonexistentEmail).WithPassword(IncorrectPassword).Generate()));
        }

        [Fact]
        [Unit("AuthenticationService")]
        public void Should_ReturnIdFromToken()
        {
            var token = _authenticationService.AuthenticateUser(_authCredentialsGenerator.WithEmail(ExistingEmail).WithPassword(CorrectPassword).Generate());
            var id = _authenticationService.GetUserIdFromJwtToken(token.AccessToken!);

            Assert.Equal(TestId, id);
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
            var token = _authenticationService.AuthenticateUser(_authCredentialsGenerator.WithEmail(ExistingEmail).WithPassword(CorrectPassword).Generate());
            var valid = _authenticationService.ValidateJwtToken(token.AccessToken!, out var user);

            Assert.NotNull(token);
            Assert.True(valid);
            Assert.NotNull(user);
            Assert.True(user.Id == TestId);
            Assert.True(user.Email == ExistingEmail);
        }
    }
}
