using Microsoft.AspNetCore.TestHost;

namespace TechCraftsmen.User.Tests.Utils;

public class TestApiClient
{
    private readonly HttpClient _httpClient;

    public TestApiClient(TestServer server)
    {
        _httpClient = server.CreateClient();
    }

    public void Authorize()
    {
        // TODO Authorize
    }

    public async Task<HttpResponseMessage> Get(string url)
    {
        // TODO Get

        throw new NotImplementedException();
    }

    public async Task<HttpResponseMessage> Post(string url, object? content = null)
    {
        // TODO Post

        throw new NotImplementedException();
    }

    public async Task<HttpResponseMessage> Put(string url, object? content = null)
    {
        // TODO Put

        throw new NotImplementedException();
    }

    public async Task<HttpResponseMessage> Patch(string url, object? content = null)
    {
        // TODO Patch

        throw new NotImplementedException();
    }

    public async Task<HttpResponseMessage> Delete(string url)
    {
        // TODO Delete

        throw new NotImplementedException();
    }
}
