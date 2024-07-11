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

        public HealthCheckTests(WebApplicationFactory<Program> factory)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", TEST_ENVIRONMENT);

            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async void ShouldReturnOK()
        {
            var route = "/HealthCheck";

            var response = await _client.GetAsync(route);

            var body = await response.Content.ReadAsStringAsync();

            var dto = JsonConvert.DeserializeObject<ResultDto<string>>(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(dto);
            Assert.Equal("Hello world!", dto.Data);
            Assert.Equal("User Api ON", dto.Message);
        }
    }
}