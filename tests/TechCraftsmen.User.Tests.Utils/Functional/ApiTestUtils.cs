using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using TechCraftsmen.User.Api;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Entities;
using TechCraftsmen.User.Core.Exceptions;
using Xunit;

namespace TechCraftsmen.User.Tests.Utils
{

    public class ApiTestUtils : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        private const string TEST_ENVIRONMENT = "Local";
        private const string AUTHENTICATE_USER_ROUTE = "/Authentication/User";

        public ApiTestUtils(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        public async Task<string> Authorize(AuthenticationCredentialsDto credentials, string environment = TEST_ENVIRONMENT)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);

            var payload = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(AUTHENTICATE_USER_ROUTE, payload);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new NotAllowedException("Could not authenticate with the provided credentials");
            }

            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResultDto<AuthenticationToken>>(body);

            return result!.Data!.AccessToken!;
        }
    }
}
