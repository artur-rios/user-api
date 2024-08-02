using Newtonsoft.Json;
using System.Net;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Tests.Utils.Functional;
using TechCraftsmen.User.Tests.Utils.Mock;

namespace TechCraftsmen.User.Api.Tests
{
    public class SecurityTests : BaseFunctionalTest, IAsyncLifetime
    {
        private readonly ApiTestUtils _testUtils;
        private readonly UserMocks _userMocks = new();
        private const string UserRoute = "/User";

        public SecurityTests() : base("Production")
        {
            _testUtils = new ApiTestUtils(_factory);
        }

        public async Task InitializeAsync()
        {
            var credentials = new AuthenticationCredentialsDto()
            {
                Email = _userMocks.TestUser.Email,
                Password = _userMocks.TestPassword
            };

            var authToken = await _testUtils.Authorize(credentials);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        /*
            Important:
                - This test will only run with you run it alone
                - If runned along with the other tests the ASPNETCORE_ENVIRONMENT will never be production
        */
        [Fact]
        public async void Should_ReturnBadRequest_IfAuthenticatedWithTestUserOutsideTestEnvironment()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                var response = await _client.GetAsync($"{UserRoute}/{_userMocks.TEST_ID}");

                var body = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<DataResultDto<string>>(body);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.NotNull(result);
                Assert.Equal("Test user can't be used outside of test environment", result.Message);
            }
        }
    }
}
