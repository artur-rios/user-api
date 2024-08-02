using Newtonsoft.Json;
using System.Net;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Tests.Utils;
using TechCraftsmen.User.Tests.Utils.Functional;
using TechCraftsmen.User.Tests.Utils.Mock;

namespace TechCraftsmen.User.Api.Tests
{
    public class UserTests : BaseFunctionalTest, IAsyncLifetime
    {
        private readonly ApiTestUtils _testUtils;
        private readonly UserMocks _userMocks;
        private const string USER_ROUTE = "/User";

        public UserTests()
        {
            _testUtils = new ApiTestUtils(_factory);
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

            var result = JsonConvert.DeserializeObject<DataResultDto<UserDto>>(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal("User retrieved with success", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(_userMocks.TestUser.Name, result.Data.Name);
            Assert.Equal(_userMocks.TestUser.Email, result.Data.Email);
            Assert.Equal(_userMocks.TestUser.RoleId, result.Data.RoleId);
            Assert.True(result.Data.Active);
        }

        [Fact]
        public async void Should_GetUsersByFilter()
        {
            var query = $"?Email={_userMocks.TestUser.Email}";

            var response = await _client.GetAsync($"{USER_ROUTE}/Filter{query}");

            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<DataResultDto<IList<UserDto>>>(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal("Search completed with success", result.Message);
            Assert.NotNull(result.Data);

            var userFound = result.Data.FirstOrDefault();

            Assert.Equal(_userMocks.TestUser.Name, userFound?.Name);
            Assert.Equal(_userMocks.TestUser.Email, userFound?.Email);
            Assert.Equal(_userMocks.TestUser.RoleId, userFound?.RoleId);
        }
    }
}
