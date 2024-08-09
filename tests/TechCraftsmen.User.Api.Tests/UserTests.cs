using Newtonsoft.Json;
using System.Net;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Tests.Utils.Functional;
using TechCraftsmen.User.Tests.Utils.Mock;

namespace TechCraftsmen.User.Api.Tests
{
    public class UserTests : BaseFunctionalTest, IAsyncLifetime
    {
        private readonly UserMocks _userMocks = new();
        private const string UserRoute = "/User";

        public async Task InitializeAsync()
        {
            AuthenticationCredentialsDto credentials = new()
            {
                Email = _userMocks.TestUser.Email,
                Password = UserMocks.TEST_PASSWORD
            };

            string authToken = await Authorize(credentials);

            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
        
        [Fact]
        public async void Should_GetUsersByFilter()
        {
            string query = $"?Email={_userMocks.TestUser.Email}";

            HttpResponseMessage response = await Client.GetAsync($"{UserRoute}/Filter{query}");

            string body = await response.Content.ReadAsStringAsync();

            DataResultDto<IList<UserDto>>? result = JsonConvert.DeserializeObject<DataResultDto<IList<UserDto>>>(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal("Search completed with success", result.Messages.First());
            Assert.NotNull(result.Data);

            UserDto? userFound = result.Data.FirstOrDefault();

            Assert.Equal(_userMocks.TestUser.Name, userFound?.Name);
            Assert.Equal(_userMocks.TestUser.Email, userFound?.Email);
            Assert.Equal(_userMocks.TestUser.RoleId, userFound?.RoleId);
        }
    }
}
