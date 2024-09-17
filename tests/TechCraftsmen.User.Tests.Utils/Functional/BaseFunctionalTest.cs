using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TechCraftsmen.User.Api;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Entities;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Extensions;

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
            DataResultDto<AuthenticationToken>? result =
                await Post<AuthenticationToken>(AuthenticateUserRoute, credentials);

            return result?.Data is not null ? result.Data.AccessToken! : throw new CustomException(result?.Messages ?? [], "Error on Authorize");
        }

        protected async Task<DataResultDto<T>?> Get<T>(string route)
        {
            HttpResponseMessage response = await Client.GetAsync(route);

            string body = await response.Content.ReadAsStringAsync();

            DataResultDto<T>? result = JsonConvert.DeserializeObject<DataResultDto<T>>(body);
            
            return result ?? throw new CustomException(["Unknown error"], "Error on Get request");
        }

        protected async Task<DataResultDto<T>?> Patch<T>(string route, object? payloadObject = null)
        {
            StringContent? payload = payloadObject?.ToJsonStringContent();

            HttpResponseMessage response = await Client.PatchAsync(route, payload);

            string body = await response.Content.ReadAsStringAsync();

            DataResultDto<T>? result = JsonConvert.DeserializeObject<DataResultDto<T>>(body);

            return result ?? throw new CustomException(["Unknown error"], "Error on Patch request");
        }

        protected async Task<DataResultDto<T>?> Post<T>(string route, object payloadObject)
        {
            StringContent payload = payloadObject.ToJsonStringContent();

            HttpResponseMessage response = await Client.PostAsync(route, payload);

            string body = await response.Content.ReadAsStringAsync();

            DataResultDto<T>? result = JsonConvert.DeserializeObject<DataResultDto<T>>(body);

            return result ?? throw new CustomException(["Unknown error"], "Error on Post request");
        }

        protected async Task<DataResultDto<T>?> Put<T>(string route, object payloadObject)
        {
            StringContent payload = payloadObject.ToJsonStringContent();

            HttpResponseMessage response = await Client.PutAsync(route, payload);

            string body = await response.Content.ReadAsStringAsync();

            DataResultDto<T>? result = JsonConvert.DeserializeObject<DataResultDto<T>>(body);

            return result ?? throw new CustomException(["Unknown error"], "Error on Put request");
        }

        protected async Task<DataResultDto<T>?> Delete<T>(string route)
        {
            HttpResponseMessage response = await Client.DeleteAsync(route);

            string body = await response.Content.ReadAsStringAsync();

            DataResultDto<T>? result = JsonConvert.DeserializeObject<DataResultDto<T>>(body);
            
            return result ?? throw new CustomException(["Unknown error"], "Error on Delete request");
        }
    }
}