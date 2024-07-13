using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Api.Tests
{
    public class HealthCheckTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        private const string TEST_ENVIRONMENT = "Local";
        private const string HEALTH_CHECK_ROUTE = "/HealthCheck";

        public HealthCheckTests(WebApplicationFactory<Program> factory)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", TEST_ENVIRONMENT);

            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async void Should_DoHealthCheck_And_ReturnSuccess()
        {
            var response = await _client.GetAsync(HEALTH_CHECK_ROUTE);

            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResultDto<string>>(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal("Hello world!", result.Data);
            Assert.Equal("User Api ON", result.Message);
        }
    }
}