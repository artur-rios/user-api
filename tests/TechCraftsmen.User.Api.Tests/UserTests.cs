using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Extensions;
using TechCraftsmen.User.Tests.Utils.Functional;
using TechCraftsmen.User.Tests.Utils.Generators;
using TechCraftsmen.User.Tests.Utils.Mock;

namespace TechCraftsmen.User.Api.Tests
{
    public class UserTests : BaseFunctionalTest, IAsyncLifetime
    {
        private readonly UserDtoGenerator _dtoGenerator = new();
        private readonly UserMocks _userMocks = new();
        private const string UserRoute = "/User";

        public async Task InitializeAsync()
        {
            AuthenticationCredentialsDto credentials = new()
            {
                Email = _userMocks.TestUser.Email, Password = UserMocks.TEST_PASSWORD
            };

            string authToken = await Authorize(credentials);

            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        [Fact]
        public async void Should_GetUsersByFilter()
        {
            string query = $"?Email={_userMocks.TestUser.Email}";

            DataResultDto<IList<UserDto>>? result = await Get<IList<UserDto>>($"{UserRoute}/Filter{query}");

            Assert.NotNull(result);
            Assert.Equal("Search completed with success", result.Messages.First());
            Assert.NotNull(result.Data);

            UserDto? userFound = result.Data.FirstOrDefault();

            Assert.Equal(_userMocks.TestUser.Name, userFound?.Name);
            Assert.Equal(_userMocks.TestUser.Email, userFound?.Email);
            Assert.Equal(_userMocks.TestUser.RoleId, userFound?.RoleId);
        }

        [Fact]
        public async void Should_CreateDeactivateAndDeleteUser()
        {
            UserDto dto = _dtoGenerator.WithEmail("creation.test@mail.com").WithDefaultName().WithRandomPassword()
                .WithRoleId((int)Roles.Regular).Generate();

            DataResultDto<int>? creationResult = await Post<int>($"{UserRoute}/Create", dto);

            Assert.NotNull(creationResult);
            Assert.True(creationResult.Data > 0);
            Assert.Equal("User created with success", creationResult.Messages.First());

            DataResultDto<int>? deactivationResult = await Patch<int>($"{UserRoute}/{creationResult.Data}/Deactivate");
            
            Assert.NotNull(deactivationResult);
            Assert.True(deactivationResult.Data > 0);
            Assert.Equal(creationResult.Data, deactivationResult.Data);
            Assert.Equal("User deactivated with success", deactivationResult.Messages.First());

            DataResultDto<int>? deletionResult = await Delete<int>($"{UserRoute}/{creationResult.Data}/Delete");
            
            Assert.NotNull(deletionResult);
            Assert.True(deletionResult.Data > 0);
            Assert.Equal(creationResult.Data, deletionResult.Data);
            Assert.Equal("User deleted with success", deletionResult.Messages.First());
        }

        [Fact]
        public async void Should_DeactivateAndActivateUser()
        {
            DataResultDto<int>? deactivationResult = await Patch<int>($"{UserRoute}/{UserMocks.TEST_ID}/Deactivate");
            
            Assert.NotNull(deactivationResult);
            Assert.True(deactivationResult.Data > 0);
            Assert.Equal(UserMocks.TEST_ID, deactivationResult.Data);
            Assert.Equal("User deactivated with success", deactivationResult.Messages.First());

            DataResultDto<int>? activationResult = await Patch<int>($"{UserRoute}/{UserMocks.TEST_ID}/Activate");
            
            Assert.NotNull(activationResult);
            Assert.True(activationResult.Data > 0);
            Assert.Equal(UserMocks.TEST_ID, activationResult.Data);
            Assert.Equal("User activated with success", activationResult.Messages.First());
        }

        [Fact]
        public async void Should_UpdateUser()
        {
            UserDto userToUpdate = _userMocks.TestUserDto.Clone() ?? throw new CustomException(["Extension method Clone returned null"], "Could not clone UserDto object");
            userToUpdate.Name = "Updated name";

            DataResultDto<UserDto>? updateResult = await Put<UserDto>($"{UserRoute}/Update", userToUpdate);
            
            Assert.NotNull(updateResult);
            Assert.Equal(userToUpdate.Id, updateResult.Data.Id);
            Assert.Equal(userToUpdate.Name, updateResult.Data.Name);
            Assert.Equal("User updated with success", updateResult.Messages.First());

            DataResultDto<IList<UserDto>>? updatedUserResult = await Get<IList<UserDto>>($"{UserRoute}/Filter?Id={userToUpdate.Id}");

            Assert.NotNull(updatedUserResult);
            Assert.NotEmpty(updatedUserResult.Data);

            UserDto updatedUser = updatedUserResult.Data.First();
            
            Assert.Equal(userToUpdate.Id, updatedUser.Id);
            Assert.Equal(userToUpdate.Name, updatedUser.Name);

            DataResultDto<UserDto>? rollbackResult = await Put<UserDto>($"{UserRoute}/Update", _userMocks.TestUserDto);
            
            Assert.NotNull(rollbackResult);
            Assert.Equal(_userMocks.TestUserDto.Id, rollbackResult.Data.Id);
            Assert.Equal(_userMocks.TestUserDto.Name, rollbackResult.Data.Name);
            Assert.Equal("User updated with success", rollbackResult.Messages.First());
            
            DataResultDto<IList<UserDto>>? rollbackUserResult = await Get<IList<UserDto>>($"{UserRoute}/Filter?Id={userToUpdate.Id}");

            Assert.NotNull(rollbackUserResult);
            Assert.NotEmpty(rollbackUserResult.Data);

            UserDto rollbackUser = rollbackUserResult.Data.First();
            
            Assert.Equal(_userMocks.TestUserDto.Id, rollbackUser.Id);
            Assert.Equal(_userMocks.TestUserDto.Name, rollbackUser.Name);
        }
    }
}