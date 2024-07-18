using Newtonsoft.Json;
using System.Net;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Tests.Utils.Functional;

namespace TechCraftsmen.User.Api.Tests
{
    public class HealthCheckTests : BaseFunctionalTest
    {
        private const string HEALTH_CHECK_ROUTE = "/HealthCheck";

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