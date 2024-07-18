using Microsoft.AspNetCore.Mvc.Testing;
using TechCraftsmen.User.Api;

namespace TechCraftsmen.User.Tests.Utils.Functional
{
    public class BaseFunctionalTest
    {
        protected readonly WebApplicationFactory<Program> _factory = new();
        protected readonly HttpClient _client;
        private const string LOCAL_ENVIRONMENT = "Local";

        public BaseFunctionalTest(string environment = LOCAL_ENVIRONMENT)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);

            _client = _factory.CreateClient();
        }
    }
}
