using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Tests.Utils;
using TechCraftsmen.User.Tests.Utils.Mock;

namespace TechCraftsmen.User.Api.Tests
{
    public class SecurityTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        private readonly ApiTestUtils _testUtils;
        private readonly UserMocks _userMocks;

        private const string PRODUCTION_ENVIRONMENT = "Production";
        private const string USER_ROUTE = "/User";

        public SecurityTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();

            _testUtils = new ApiTestUtils(factory);
            _userMocks = new UserMocks();
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

        /* Important:
            - This test can't run along with the others, because it needs to change the ASPNETCORE_ENVIRONMENT.
            - Once the other tests already set it's value, it does not change during excution and cause this test to fail.
            - This test will only work properly if run alone.
        */
        [Fact]
        public async void Should_ReturnBadRequest_IfAuthenticatedWithTestUserOutsideTestEnvironment()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", PRODUCTION_ENVIRONMENT);

            var response = await _client.GetAsync($"{USER_ROUTE}/{_userMocks.TEST_ID}");

            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResultDto<string>>(body);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal("Test user can't be used outside of test environment", result.Message);
        }
    }
}
