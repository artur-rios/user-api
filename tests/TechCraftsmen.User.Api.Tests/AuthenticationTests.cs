﻿using Newtonsoft.Json;
using System.Net;
using System.Text;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Entities;
using TechCraftsmen.User.Tests.Utils.Functional;
using TechCraftsmen.User.Tests.Utils.Generators;
using TechCraftsmen.User.Tests.Utils.Mock;

namespace TechCraftsmen.User.Api.Tests
{
    public class AuthenticationTests : BaseFunctionalTest
    {
        private readonly UserMocks _userMocks = new();
        private readonly RandomStringGenerator _randomStringGenerator = new();
        private readonly EmailGenerator _emailGenerator = new();
        private const string AuthenticateUserRoute = "/Authentication/User";

        [Fact]
        public async void Should_AuthenticateUser()
        {
            var credentialsDto = new AuthenticationCredentialsDto()
            {
                Email = _userMocks.TestUser.Email,
                Password = _userMocks.TestPassword
            };

            var payload = new StringContent(JsonConvert.SerializeObject(credentialsDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(AuthenticateUserRoute, payload);

            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<DataResultDto<AuthenticationToken>>(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result.Data.AccessToken));
            Assert.True(result.Data.Authenticated);
            Assert.Equal("User authenticated with success", result.Message);
        }

        [Fact]
        public async void Should_NotAuthenticate_ForIncorrectCredentials()
        {
            List<AuthenticationCredentialsDto> incorrectCredentials = [
                new(_userMocks.TestUser.Email, _randomStringGenerator.WithLength(8).WithLowerChars().WithNumbers().WithUpperChars().DifferentFrom([_userMocks.TestPassword]).Generate()),
            new(_emailGenerator.Generate(), _userMocks.TestPassword),
            new(_emailGenerator.Generate(), _randomStringGenerator.WithLength(8).WithLowerChars().WithNumbers().WithUpperChars().DifferentFrom([_userMocks.TestPassword]).Generate())
            ];

            foreach (var credentialDto in incorrectCredentials)
            {
                var payload = new StringContent(JsonConvert.SerializeObject(credentialDto), Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(AuthenticateUserRoute, payload);

                var body = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<DataResultDto<string>>(body);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.NotNull(result);
                Assert.Equal("Invalid credentials", result.Message);
            }
        }

        [Fact]
        public async void Should_NotAuthenticate_ForInvalidCredentials()
        {
            List<AuthenticationCredentialsDto> invalidCredentials = [
                new(string.Empty, _userMocks.TestPassword),
            new(_userMocks.TestUser.Email, string.Empty),
            new()
            ];

            foreach (var credentialDto in invalidCredentials)
            {
                var payload = new StringContent(JsonConvert.SerializeObject(credentialDto), Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(AuthenticateUserRoute, payload);

                var body = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<DataResultDto<string>>(body);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.NotNull(result);

                var emailValidationError = "'Email' must not be empty.";
                var passwordValidationError = "'Password' must not be empty.";
                var expectedValidationError = string.Empty;

                if (string.IsNullOrEmpty(credentialDto.Email) && string.IsNullOrEmpty(credentialDto.Password))
                {
                    expectedValidationError = $"{emailValidationError} | {passwordValidationError}";
                }
                else if (string.IsNullOrEmpty(credentialDto.Email))
                {
                    expectedValidationError = emailValidationError;
                }
                else if (string.IsNullOrEmpty(credentialDto.Password))
                {
                    expectedValidationError = passwordValidationError;
                }

                Assert.Equal(expectedValidationError, result.Message);
            }
        }
    }
}