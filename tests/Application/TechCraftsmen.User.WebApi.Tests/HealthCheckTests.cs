using Newtonsoft.Json;
using System.Net;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Tests.Configuration.Attributes;
using TechCraftsmen.User.Tests.Configuration.Functional;

namespace TechCraftsmen.User.WebApi.Tests
{
    public class HealthCheckTests : BaseFunctionalTest
    {
        private const string HealthCheckRoute = "/HealthCheck";

        [FunctionalFact("HealthCheck")]
        public async void Should_DoHealthCheck_And_ReturnSuccess()
        {
            HttpResponseMessage response = await Client.GetAsync(HealthCheckRoute);

            string body = await response.Content.ReadAsStringAsync();

            DataResultDto<string>? result = JsonConvert.DeserializeObject<DataResultDto<string>>(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal("Hello world!", result.Data);
            Assert.Equal("User Api ON", result.Messages.First());
        }
    }
}