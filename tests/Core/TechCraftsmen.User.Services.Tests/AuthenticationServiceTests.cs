using Microsoft.Extensions.Options;
using Moq;
using TechCraftsmen.User.Domain.Interfaces;
using TechCraftsmen.User.Services.Authentication;
using TechCraftsmen.User.Services.Configuration;
using TechCraftsmen.User.Services.Dto;
using TechCraftsmen.User.Services.Enums;
using TechCraftsmen.User.Services.Implementations;
using TechCraftsmen.User.Services.Output;
using TechCraftsmen.User.Services.Validation;
using TechCraftsmen.User.Tests.Configuration.Attributes;
using TechCraftsmen.User.Tests.Mock.Generators;
using TechCraftsmen.User.Utils.Exceptions;
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
        private readonly UserGenerator _userGenerator = new();
        private readonly Mock<ICrudRepository<Domain.Entities.User>> _userRepository;

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
            _userRepository = new Mock<ICrudRepository<Domain.Entities.User>>();

            _authTokenConfig.Setup(config => config.Value).Returns(_authTokenConfigGenerator.WithAudience("audience")
                .WithIssuer("issuer").WithSeconds(60).WithSecret(TestTokenSecret).Generate());

            _userRepository.Setup(repo => repo.GetByFilter(It.IsAny<Dictionary<string, object>>(), It.IsAny<bool>()))
                .Returns((Dictionary<string, object> filter, bool track) =>
                {
                   filter.TryGetValue("Id", out object? id);
                   filter.TryGetValue("Email", out object? email);

                   bool isTestId = id is not null && (int)id == TestId;
                   bool isExistingEmail = email is not null && (string)email == ExistingEmail;

                   if (isTestId || isExistingEmail)
                   {
                       return new List<Domain.Entities.User>
                       {
                           _userGenerator.WithId(TestId).WithEmail(ExistingEmail).WithPassword(CorrectPassword)
                               .Generate()
                       }.AsQueryable();
                   }

                   return new List<Domain.Entities.User>().AsQueryable();
                });

            _authenticationService = new AuthenticationService(_authTokenConfig.Object, _authCredentialsValidator,
                _authTokenConfigValidator, _userRepository.Object);
        }
        
        [UnitFact("AuthenticationService")]
        public void Should_ThrowValidationException_IfAuthTokenConfigIsMissing()
        {
            _authTokenConfig.Setup(config => config.Value).Returns(_authTokenConfigGenerator.WithAudience("audience")
                .WithIssuer("issuer").WithSeconds(0).WithSecret("").Generate());

            Assert.Throws<CustomException>(() => new AuthenticationService(_authTokenConfig.Object,
                _authCredentialsValidator, _authTokenConfigValidator, _userRepository.Object));
        }

        [UnitFact("AuthenticationService")]
        public void Should_ThrowValidationException_ForInvalidCredentials()
        {
            ServiceOutput<AuthenticationToken?> result = _authenticationService.AuthenticateUser(_authCredentialsGenerator.WithEmail(string.Empty)
                .WithPassword(string.Empty).Generate());
            
            Assert.Null(result.Data);
            Assert.Equal(Results.ValidationError, result.Result);
            Assert.Equal(2, result.Messages.Length);
            Assert.Equal("'Email' must not be empty.", result.Messages.First());
            Assert.Equal("'Password' must not be empty.", result.Messages[1]);
        }
        
        [UnitFact("AuthenticationService")]
        public void Should_ReturnNotAllowedError_ForNonexistentEmail()
        {
            ServiceOutput<AuthenticationToken?> result = _authenticationService.AuthenticateUser(
                _authCredentialsGenerator.WithEmail(NonexistentEmail)
                    .WithPassword(CorrectPassword).Generate());

            Assert.Null(result.Data);
            Assert.Equal(Results.NotAllowed, result.Result);
            Assert.Equal("Invalid credentials", result.Messages.First());
        }

        [UnitFact("AuthenticationService")]
        public void Should_ReturnNotAllowedError_ForIncorrectPassword()
        {
            ServiceOutput<AuthenticationToken?> result = _authenticationService.AuthenticateUser(
                _authCredentialsGenerator.WithEmail(ExistingEmail)
                    .WithPassword(IncorrectPassword).Generate());

            Assert.Null(result.Data);
            Assert.Equal(Results.NotAllowed, result.Result);
            Assert.Equal("Invalid credentials", result.Messages.First());
        }
        
        [UnitFact("AuthenticationService")]
        public void Should_ReturnNotAllowedError_ForIncorrectCredentials()
        {
            ServiceOutput<AuthenticationToken?> result = _authenticationService.AuthenticateUser(
                _authCredentialsGenerator
                    .WithEmail(NonexistentEmail).WithPassword(IncorrectPassword).Generate());

            Assert.Null(result.Data);
            Assert.Equal(Results.NotAllowed, result.Result);
            Assert.Equal("Invalid credentials", result.Messages.First());
        }
        
        [UnitTheory("UserCreationRule")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage",
            "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_ReturnFalse_ForInvalidToken(string token)
        {
            ServiceOutput<bool> result = _authenticationService.ValidateJwtToken(token, out UserDto? user);

            Assert.False(result.Data);
            Assert.Equal(Results.Success, result.Result);
            Assert.Equal("Invalid auth token", result.Messages.First());

            Assert.Null(user);
        }
        
        [UnitFact("AuthenticationService")]
        public void Should_GenerateValidToken_ForValidCredentials()
        {
            ServiceOutput<AuthenticationToken?> result = _authenticationService.AuthenticateUser(
                _authCredentialsGenerator
                    .WithEmail(ExistingEmail).WithPassword(CorrectPassword).Generate());

            Assert.NotNull(result.Data);
            Assert.Equal(Results.Success, result.Result);
            Assert.Equal("User authenticated with success", result.Messages.First());

            ServiceOutput<bool> validationResult =
                _authenticationService.ValidateJwtToken(result.Data.AccessToken!, out UserDto? user);

            Assert.True(validationResult.Data);
            Assert.Equal(Results.Success, validationResult.Result);
            Assert.Equal("Auth token is valid", validationResult.Messages.First());

            Assert.NotNull(user);
            Assert.True(user.Id == TestId);
            Assert.True(user.Email == ExistingEmail);
        }
    }
}