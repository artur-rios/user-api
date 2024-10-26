using Newtonsoft.Json;
using System.Net;
using TechCraftsmen.User.Services.Dto;
using TechCraftsmen.User.Tests.Configuration.Attributes;
using TechCraftsmen.User.Tests.Configuration.Functional;
using TechCraftsmen.User.Tests.Mock.Data;
using TechCraftsmen.User.WebApi.Controllers;

namespace TechCraftsmen.User.WebApi.Tests
{
    public class SecurityTests() : BaseFunctionalTest("Production"), IAsyncLifetime
    {
        private readonly UserMockData _userMocks = new();
        private const string UserRoute = "/User";

        public async Task InitializeAsync()
        {
            AuthenticationCredentialsDto credentials = new()
            {
                Email = _userMocks.TestUser.Email,
                Password = UserMockData.TEST_PASSWORD
            };

            string authToken = await Authorize(credentials);

            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
        
        [FunctionalFact("Security")]
        public async void Should_ReturnBadRequest_IfAuthenticatedWithTestUserOutsideTestEnvironment()
        {
            const string filter = "Filter?Id=4"; // {UserMocks.TEST_ID}

            HttpResponseMessage response = await Client.GetAsync($"{UserRoute}/{filter}");

            string body = await response.Content.ReadAsStringAsync();

            BaseController.Output<string>? result = JsonConvert.DeserializeObject<BaseController.Output<string>>(body);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.NotNull(response);
            Assert.False(result!.Success);
            Assert.Equal("Test user can't be used outside of test environment", result.Messages.First());
        }
    }
}
