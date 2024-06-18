using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace TechCraftsmen.User.Tests.Utils.Functional
{
    public class TestProgram
    {
        private readonly IConfiguration _configuration;

        public TestProgram()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .Build();
        }

        public IWebHostBuilder CreateWebHostBuilder() => new WebHostBuilder()
            // TODO .UseStartup<Startup>()
            .UseEnvironment("Testing")
            .ConfigureAppConfiguration((builder, config) =>
            {
                config.AddConfiguration(_configuration);
            });
    }
}
