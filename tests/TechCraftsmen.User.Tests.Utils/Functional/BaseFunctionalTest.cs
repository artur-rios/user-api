using Microsoft.AspNetCore.Mvc.Testing;
using TechCraftsmen.User.Api;

namespace TechCraftsmen.User.Tests.Utils.Functional
{
    public class BaseFunctionalTest
    {
        protected readonly WebApplicationFactory<Program> Factory = new();
        protected readonly HttpClient Client;
        private const string LocalEnvironment = "Local";

        public BaseFunctionalTest(string environment = LocalEnvironment)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);

            Client = Factory.CreateClient();
        }
    }
}
