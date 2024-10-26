using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TechCraftsmen.User.Services.Authentication;
using TechCraftsmen.User.Services.Dto;
using TechCraftsmen.User.Utils;
using TechCraftsmen.User.Utils.Exceptions;
using TechCraftsmen.User.Utils.Extensions;
using TechCraftsmen.User.WebApi;
using TechCraftsmen.User.WebApi.Controllers;

namespace TechCraftsmen.User.Tests.Configuration.Functional
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
            BaseController.Output<AuthenticationToken>? result =
                await Post<AuthenticationToken>(AuthenticateUserRoute, credentials);

            return result?.Data is not null ? result.Data.AccessToken! : throw new CustomException(result?.Messages ?? [], "Error on Authorize");
        }

        protected async Task<BaseController.Output<T>?> Get<T>(string route)
        {
            HttpResponseMessage response = await Client.GetAsync(route);

            string body = await response.Content.ReadAsStringAsync();

            BaseController.Output<T>? result = JsonConvert.DeserializeObject<BaseController.Output<T>>(body);
            
            return result ?? throw new CustomException(["Unknown error"], "Error on Get request");
        }

        protected async Task<BaseController.Output<T>?> Patch<T>(string route, object? payloadObject = null)
        {
            StringContent? payload = payloadObject?.ToJsonStringContent();

            HttpResponseMessage response = await Client.PatchAsync(route, payload);

            string body = await response.Content.ReadAsStringAsync();

            BaseController.Output<T>? result = JsonConvert.DeserializeObject<BaseController.Output<T>>(body);

            return result ?? throw new CustomException(["Unknown error"], "Error on Patch request");
        }

        protected async Task<BaseController.Output<T>?> Post<T>(string route, object payloadObject)
        {
            StringContent payload = payloadObject.ToJsonStringContent();

            HttpResponseMessage response = await Client.PostAsync(route, payload);

            string body = await response.Content.ReadAsStringAsync();

            BaseController.Output<T>? result = JsonConvert.DeserializeObject<BaseController.Output<T>>(body);

            return result ?? throw new CustomException(["Unknown error"], "Error on Post request");
        }

        protected async Task<BaseController.Output<T>?> Put<T>(string route, object payloadObject)
        {
            StringContent payload = payloadObject.ToJsonStringContent();

            HttpResponseMessage response = await Client.PutAsync(route, payload);

            string body = await response.Content.ReadAsStringAsync();

            BaseController.Output<T>? result = JsonConvert.DeserializeObject<BaseController.Output<T>>(body);

            return result ?? throw new CustomException(["Unknown error"], "Error on Put request");
        }

        protected async Task<BaseController.Output<T>?> Delete<T>(string route)
        {
            HttpResponseMessage response = await Client.DeleteAsync(route);

            string body = await response.Content.ReadAsStringAsync();

            BaseController.Output<T>? result = JsonConvert.DeserializeObject<BaseController.Output<T>>(body);
            
            return result ?? throw new CustomException(["Unknown error"], "Error on Delete request");
        }
    }
}