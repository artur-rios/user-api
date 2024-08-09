using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using TechCraftsmen.User.Api;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Entities;
using TechCraftsmen.User.Core.Exceptions;

namespace TechCraftsmen.User.Tests.Utils.Functional
{
    public class BaseFunctionalTest
    {
        protected readonly HttpClient Client;
        private readonly WebApplicationFactory<Program> _factory = new();

        private const string AuthenticateUserRoute = "/Authentication/User";
        private const string LocalEnvironment = "Local";

        protected BaseFunctionalTest(string environment = LocalEnvironment)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);

            Client = _factory.CreateClient();
        }

        protected async Task<string> Authorize(AuthenticationCredentialsDto credentials)
        {
            StringContent payload = new(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await Client.PostAsync(AuthenticateUserRoute, payload);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new CustomException(["Could not authenticate with the provided credentials"]);
            }

            string body = await response.Content.ReadAsStringAsync();

            DataResultDto<AuthenticationToken>? result =
                JsonConvert.DeserializeObject<DataResultDto<AuthenticationToken>>(body);

            return result!.Data.AccessToken!;
        }
    }
}