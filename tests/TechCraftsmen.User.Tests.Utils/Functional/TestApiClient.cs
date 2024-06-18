using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Extensions;
using TechCraftsmen.User.Core.Interfaces.Services;

namespace TechCraftsmen.User.Tests.Utils.Functional
{
    public class TestApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TestApiClient> _logger;
        private readonly IAuthenticationService _authService;

        public TestApiClient(TestServer server, IAuthenticationService authService, ILogger<TestApiClient> logger)
        {
            _authService = authService;
            _httpClient = server.CreateClient();
            _logger = logger;
        }

        public void Authorize(AuthenticationCredentialsDto authCredentials)
        {
            try
            {
                var authToken = _authService.AuthenticateUser(authCredentials);

                _httpClient.DefaultRequestHeaders.Add("Authorization", authToken.AccessToken);
            }
            catch (Exception e)
            {
#pragma warning disable CA2254 // Template should be a static expression
                _logger.LogError(e.Message);
#pragma warning restore CA2254 // Template should be a static expression
            }
        }

        public async Task<HttpResponseMessage> Get(string url)
        {
            return await _httpClient.GetAsync(url);
        }

        public async Task<HttpResponseMessage> Post(string url, object? content = null)
        {
            return await _httpClient.PostAsync(url, content?.ToJsonContent());
        }

        public async Task<HttpResponseMessage> Put(string url, object? content = null)
        {
            return await _httpClient.PutAsync(url, content?.ToJsonContent());
        }

        public async Task<HttpResponseMessage> Patch(string url, object? content = null)
        {
            return await _httpClient.PatchAsync(url, content?.ToJsonContent());
        }

        public async Task<HttpResponseMessage> Delete(string url)
        {
            return await _httpClient.DeleteAsync(url);
        }
    }
}
