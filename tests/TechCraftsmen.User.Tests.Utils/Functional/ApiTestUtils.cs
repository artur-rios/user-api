using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using TechCraftsmen.User.Api;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Entities;
using TechCraftsmen.User.Core.Exceptions;
using Xunit;

namespace TechCraftsmen.User.Tests.Utils.Functional
{

    public class ApiTestUtils(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();
        private const string AuthenticateUserRoute = "/Authentication/User";

        public async Task<string> Authorize(AuthenticationCredentialsDto credentials)
        {
            StringContent payload = new(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync(AuthenticateUserRoute, payload);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new NotAllowedException("Could not authenticate with the provided credentials");
            }

            string body = await response.Content.ReadAsStringAsync();

            DataResultDto<AuthenticationToken>? result = JsonConvert.DeserializeObject<DataResultDto<AuthenticationToken>>(body);

            return result!.Data.AccessToken!;
        }
    }
}
