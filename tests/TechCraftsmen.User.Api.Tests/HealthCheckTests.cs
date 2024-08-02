using Newtonsoft.Json;
using System.Net;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Tests.Utils.Functional;

namespace TechCraftsmen.User.Api.Tests
{
    public class HealthCheckTests : BaseFunctionalTest
    {
        private const string HealthCheckRoute = "/HealthCheck";

        [Fact]
        public async void Should_DoHealthCheck_And_ReturnSuccess()
        {
            var response = await _client.GetAsync(HealthCheckRoute);

            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<DataResultDto<string>>(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal("Hello world!", result.Data);
            Assert.Equal("User Api ON", result.Message);
        }
    }
}