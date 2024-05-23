using Microsoft.AspNetCore.TestHost;

namespace TechCraftsmen.User.Tests.Utils
{

    public class TestApiServer : TestServer
    {
        public TestApiServer() : base(new TestProgram().CreateWebHostBuilder())
        {
            // TODO Test server
        }
    }
}
