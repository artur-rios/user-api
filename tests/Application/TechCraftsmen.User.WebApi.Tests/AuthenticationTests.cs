using Newtonsoft.Json;
using System.Net;
using System.Text;
using TechCraftsmen.User.Services.Authentication;
using TechCraftsmen.User.Services.Dto;
using TechCraftsmen.User.Tests.Configuration.Attributes;
using TechCraftsmen.User.Tests.Configuration.Functional;
using TechCraftsmen.User.Tests.Mock.Data;
using TechCraftsmen.User.Tests.Mock.Generators;
using TechCraftsmen.User.WebApi.Controllers;

namespace TechCraftsmen.User.WebApi.Tests
{
    public class AuthenticationTests : BaseFunctionalTest
    {
        private readonly UserMockData _userMocks = new();
        private readonly RandomStringGenerator _randomStringGenerator = new();
        private readonly EmailGenerator _emailGenerator = new();
        private const string AuthenticateUserRoute = "/Authentication/User";

        [FunctionalFact("Authentication")]
        public async void Should_AuthenticateUser()
        {
            AuthenticationCredentialsDto credentialsDto = new()
            {
                Email = _userMocks.TestUser.Email,
                Password = UserMockData.TEST_PASSWORD
            };

            StringContent payload = new(JsonConvert.SerializeObject(credentialsDto), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await Client.PostAsync(AuthenticateUserRoute, payload);

            string body = await response.Content.ReadAsStringAsync();

            BaseController.Output<AuthenticationToken>? result = JsonConvert.DeserializeObject<BaseController.Output<AuthenticationToken>>(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result.Data.AccessToken));
            Assert.True(result.Data.Authenticated);
            Assert.Equal("User authenticated with success", result.Messages.First());
        }

        [FunctionalFact("Authentication")]
        public async void Should_NotAuthenticate_ForIncorrectCredentials()
        {
            List<AuthenticationCredentialsDto> incorrectCredentials = [
                new AuthenticationCredentialsDto(_userMocks.TestUser.Email, _randomStringGenerator.WithLength(8).WithLowerChars().WithNumbers().WithUpperChars().DifferentFrom([UserMockData.TEST_PASSWORD]).Generate()),
            new AuthenticationCredentialsDto(_emailGenerator.Generate(), UserMockData.TEST_PASSWORD),
            new AuthenticationCredentialsDto(_emailGenerator.Generate(), _randomStringGenerator.WithLength(8).WithLowerChars().WithNumbers().WithUpperChars().DifferentFrom([UserMockData.TEST_PASSWORD]).Generate())
            ];

            foreach (AuthenticationCredentialsDto? credentialDto in incorrectCredentials)
            {
                StringContent payload = new(JsonConvert.SerializeObject(credentialDto), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await Client.PostAsync(AuthenticateUserRoute, payload);

                string body = await response.Content.ReadAsStringAsync();

                BaseController.Output<string>? result = JsonConvert.DeserializeObject<BaseController.Output<string>>(body);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.NotNull(result);
                Assert.Equal("Invalid credentials", result.Messages.First());
            }
        }

        [FunctionalFact("Authentication")]
        public async void Should_NotAuthenticate_ForInvalidCredentials()
        {
            List<AuthenticationCredentialsDto> invalidCredentials = [
                new AuthenticationCredentialsDto(string.Empty, UserMockData.TEST_PASSWORD),
            new AuthenticationCredentialsDto(_userMocks.TestUser.Email, string.Empty),
            new AuthenticationCredentialsDto()
            ];

            foreach (AuthenticationCredentialsDto? credentialDto in invalidCredentials)
            {
                StringContent payload = new(JsonConvert.SerializeObject(credentialDto), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await Client.PostAsync(AuthenticateUserRoute, payload);

                string body = await response.Content.ReadAsStringAsync();

                BaseController.Output<string>? result = JsonConvert.DeserializeObject<BaseController.Output<string>>(body);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.NotNull(result);

                const string emailValidationError = "'Email' must not be empty.";
                const string passwordValidationError = "'Password' must not be empty.";
                string expectedValidationError = string.Empty;

                if (string.IsNullOrEmpty(credentialDto.Email) && string.IsNullOrEmpty(credentialDto.Password))
                {
                    Assert.Equal(emailValidationError, result.Messages.First());
                    Assert.Equal(passwordValidationError, result.Messages[1]);
                }
                else
                {
                    if (string.IsNullOrEmpty(credentialDto.Email))
                    {
                        expectedValidationError = emailValidationError;
                    }
                    else if (string.IsNullOrEmpty(credentialDto.Password))
                    {
                        expectedValidationError = passwordValidationError;
                    }

                    Assert.Equal(expectedValidationError, result.Messages.First());
                }
            }
        }
    }
}