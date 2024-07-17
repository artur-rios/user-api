using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Tests.Utils;
using TechCraftsmen.User.Tests.Utils.Mock;

namespace TechCraftsmen.User.Api.Tests
{

    public class UserTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        private readonly ApiTestUtils _testUtils;
        private readonly UserMocks _userMocks;

        private const string TEST_ENVIRONMENT = "Local";
        private const string USER_ROUTE = "/User";



        public UserTests(WebApplicationFactory<Program> factory)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", TEST_ENVIRONMENT);

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

        [Fact]
        public async void Should_GetUserById()
        {
            var response = await _client.GetAsync($"{USER_ROUTE}/{_userMocks.TEST_ID}");

            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResultDto<UserDto>>(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal("User retrieved with success", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(_userMocks.TestUser.Name, result.Data.Name);
            Assert.Equal(_userMocks.TestUser.Email, result.Data.Email);
            Assert.Equal(_userMocks.TestUser.RoleId, result.Data.RoleId);
            Assert.True(result.Data.Active);
        }
    }
}
