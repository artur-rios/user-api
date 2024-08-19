using FluentValidation;
using Microsoft.AspNetCore.Http;
using Moq;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Filters;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Mapping;
using TechCraftsmen.User.Core.Validation;
using TechCraftsmen.User.Core.Validation.Fluent;
using TechCraftsmen.User.Tests.Utils.Generators;
using TechCraftsmen.User.Tests.Utils.Mock;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;
using Results = TechCraftsmen.User.Core.Enums.Results;

namespace TechCraftsmen.User.Services.Tests
{
    public class UserServiceTests
    {
        private readonly UserService _userService;

        private readonly RandomStringGenerator _randomStringGenerator = new();
        private readonly UserGenerator _userGenerator = new();
        private readonly UserDtoGenerator _userDtoGenerator = new();

        private readonly string _passwordMock;
        private readonly UserDto _userDtoMock;
        private readonly Core.Entities.User _userMock;

        private const int ExistingId = 1;
        private const int NonexistentId = 2;
        private const int InactiveId = 3;
        private const string ExistingEmail = "exists@mail.com";
        private const string NonexistentEmail = "inexists@mail.com";

        public UserServiceTests()
        {
            Mock<ICrudRepository<Core.Entities.User>> userRepository = new();
            IValidator<UserDto> userValidator = new UserDtoValidator();
            Mock<HttpContextAccessor> httpContextAccessor = new();

            _passwordMock = _randomStringGenerator.WithLength(PasswordValidator.MINIMUM_LENGTH).WithNumbers()
                .WithLowerChars().WithUpperChars().Generate();

            _userMock = _userGenerator.WithDefaultEmail().WithDefaultName().WithRandomId().WithPassword(_passwordMock)
                .WithRoleId((int)Roles.Regular).Generate();
            _userDtoMock = _userMock.ToDto();
            _userDtoMock.Password = _passwordMock;
            UserMocks userMocks = new();

            userRepository.Setup(repo => repo.Create(It.IsAny<Core.Entities.User>())).Returns(() => _userMock.Id);

            userRepository.Setup(repo => repo.GetByFilter(It.IsAny<Dictionary<string, object>>(), It.IsAny<bool>()))
                .Returns((Dictionary<string, object> filter, bool track) =>
                {
                    object value = filter.FirstOrDefault().Value;

                    if (value is int)
                    {
                        return (int)filter.FirstOrDefault().Value switch
                        {
                            ExistingId => new List<Core.Entities.User>
                            {
                                _userGenerator.WithId(ExistingId)
                                    .WithDefaultEmail()
                                    .WithDefaultName()
                                    .WithRandomPassword()
                                    .Generate()
                            }.AsQueryable(),
                            InactiveId => new List<Core.Entities.User>
                            {
                                _userGenerator.WithId(InactiveId)
                                    .WithDefaultEmail()
                                    .WithDefaultName()
                                    .WithRandomPassword()
                                    .WithStatus(false)
                                    .Generate()
                            }.AsQueryable(),
                            _ => new List<Core.Entities.User>().AsQueryable()
                        };
                    }

                    return filter.FirstOrDefault().Value as string == ExistingEmail
                        ? new List<Core.Entities.User>
                        {
                            _userGenerator.WithId(ExistingId)
                                .WithEmail(ExistingEmail)
                                .WithRandomPassword()
                                .Generate()
                        }.AsQueryable()
                        : new List<Core.Entities.User>().AsQueryable();
                });

            userRepository.Setup(repo => repo.Update(It.IsAny<Core.Entities.User>()));

            userRepository.Setup(repo => repo.Delete(It.IsAny<Core.Entities.User>()));

            httpContextAccessor.Object.HttpContext = new DefaultHttpContext();
            httpContextAccessor.Object.HttpContext!.Items =
                new Dictionary<object, object?> { { "User", userMocks.TestUserDto } };

            _userService = new UserService(userRepository.Object, httpContextAccessor.Object, userValidator);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnValidationError_When_UserDtoIsInvalid()
        {
            UserDto invalidDto = _userDtoGenerator.WithEmail("").WithDefaultName().WithRandomPassword().WithRoleId().Generate();

            OperationResultDto<int> result = _userService.CreateUser(invalidDto);
            
            Assert.Equal(default, result.Data);
            Assert.Equal(Results.ValidationError, result.Result);
            Assert.Equal("Email must not be null or empty", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnNotAllowedError_When_ForAlreadyRegisteredEmail()
        {
            UserDto userWithExistingEmail = _userDtoGenerator.WithEmail(ExistingEmail).WithDefaultName()
                .WithRandomPassword().WithRoleId().Generate();

            OperationResultDto<int> result = _userService.CreateUser(userWithExistingEmail);
            
            Assert.Equal(default, result.Data);
            Assert.Equal(Results.NotAllowed, result.Result);
            Assert.Equal("E-mail already registered", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_CreateUser_And_ReturnId_ForValidDto()
        {
            OperationResultDto<int> result = _userService.CreateUser(_userDtoMock);
            
            Assert.Equal(_userMock.Id, result.Data);
            Assert.Equal(Results.Created, result.Result);
            Assert.Equal("User created with success", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnNotFoundError_When_IdNotOnDatabase()
        {
            OperationResultDto<IList<UserDto>> result = _userService.GetUsersByFilter(new UserFilter(NonexistentId));

            Assert.Empty(result.Data!);
            Assert.Equal(Results.NotFound, result.Result);
            Assert.Equal("No users found for the given filter", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnUser_When_IdIsOnDatabase()
        {
            OperationResultDto<IList<UserDto>> result = _userService.GetUsersByFilter(new UserFilter(ExistingId));
            
            Assert.NotNull(result.Data);
            Assert.Equal(ExistingId, result.Data.First().Id);
            Assert.Equal(Results.Success, result.Result);
            Assert.Equal("Search completed with success", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnNotFoundError_When_FilterDoesNotReturnAnyResult()
        {
            OperationResultDto<IList<UserDto>> result = _userService.GetUsersByFilter(new UserFilter(NonexistentEmail));

            Assert.Empty(result.Data!);
            Assert.Equal(Results.NotFound, result.Result);
            Assert.Equal("No users found for the given filter", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnUsers_For_ValidAndExistingFilter()
        {
            OperationResultDto<IList<UserDto>> result = _userService.GetUsersByFilter(new UserFilter(ExistingEmail));

            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data);
            Assert.True(result.Data.First(user => user.Email == ExistingEmail) is not null);
            Assert.Equal(Results.Success, result.Result);
            Assert.Equal("Search completed with success", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnNotFoundError_WhenIdNotOnDatabaseAndPasswordCannotBeFound()
        {
            OperationResultDto<HashDto?> result = _userService.GetPasswordByUserId(NonexistentId);

            Assert.Equal(default, result.Data);
            Assert.Equal(Results.NotFound, result.Result);
            Assert.Equal("User not found", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnPassword_When_IdIsOnDatabase()
        {
            OperationResultDto<HashDto?> result = _userService.GetPasswordByUserId(ExistingId);

            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data.Hash);
            Assert.NotEmpty(result.Data.Salt);
            Assert.Equal(Results.Success, result.Result);
            Assert.Equal("Password found", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnNotFoundError_WhenIdNotOnDatabaseAndCannotUpdateUser()
        {
            Core.Entities.User userMock = _userGenerator.WithDefaultEmail().WithDefaultName().WithId(NonexistentId)
                .WithPassword(_passwordMock).Generate();
            UserDto userDto = userMock.ToDto();

            OperationResultDto<UserDto?> result = _userService.UpdateUser(userDto);

            Assert.Null(result.Data);
            Assert.Equal(Results.NotFound, result.Result);
            Assert.Equal("User not found", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnNotNotAllowedError_WhenUserIsInactive()
        {
            Core.Entities.User userMock = _userGenerator.WithDefaultEmail().WithDefaultName().WithId(InactiveId)
                .WithPassword(_passwordMock).Generate();
            UserDto userDto = userMock.ToDto();

            OperationResultDto<UserDto?> result = _userService.UpdateUser(userDto);

            Assert.Null(result.Data);
            Assert.Equal(Results.NotAllowed, result.Result);
            Assert.Equal("Can't update inactive user", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_UpdateUser()
        {
            Core.Entities.User userMock = _userGenerator.WithDefaultEmail().WithDefaultName().WithId(ExistingId)
                .WithPassword(_passwordMock).Generate();
            UserDto userDto = userMock.ToDto();

            OperationResultDto<UserDto?> result = _userService.UpdateUser(userDto);

            Assert.NotNull(result.Data);
            Assert.Equal(Results.Success, result.Result);
            Assert.Equal("User updated with success", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnNotFoundError_WhenIdNotOnDatabaseAndCannotActivateUser()
        {
            OperationResultDto<int> result = _userService.ActivateUser(NonexistentId);

            Assert.Equal(default, result.Data);
            Assert.Equal(Results.NotFound, result.Result);
            Assert.Equal("User not found", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnEntityNotChanged_WhenUserIsActive_On_Activation()
        {
            OperationResultDto<int> result = _userService.ActivateUser(ExistingId);

            Assert.Equal(ExistingId, result.Data);
            Assert.Equal(Results.EntityNotChanged, result.Result);
            Assert.Equal("User already active", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ActivateUser()
        {
            OperationResultDto<int> result = _userService.ActivateUser(InactiveId);

            Assert.Equal(InactiveId, result.Data);
            Assert.Equal(Results.Success, result.Result);
            Assert.Equal("User activated with success", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnNotFoundError_WhenIdNotOnDatabaseAndCannotDeactivateUser()
        {
            OperationResultDto<int> result = _userService.DeactivateUser(NonexistentId);

            Assert.Equal(default, result.Data);
            Assert.Equal(Results.NotFound, result.Result);
            Assert.Equal("User not found", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnEntityNotChanged_WhenUserIsInactive_On_Deactivation()
        {
            OperationResultDto<int> result = _userService.DeactivateUser(InactiveId);

            Assert.Equal(InactiveId, result.Data);
            Assert.Equal(Results.EntityNotChanged, result.Result);
            Assert.Equal("User already inactive", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_DeactivateUser()
        {
            OperationResultDto<int> result = _userService.DeactivateUser(ExistingId);

            Assert.Equal(ExistingId, result.Data);
            Assert.Equal(Results.Success, result.Result);
            Assert.Equal("User deactivated with success", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnNotFoundError_WhenIdNotOnDatabaseAndCannotDeleteUser()
        {
            OperationResultDto<int> result = _userService.DeleteUser(NonexistentId);

            Assert.Equal(default, result.Data);
            Assert.Equal(Results.NotFound, result.Result);
            Assert.Equal("User not found", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnNotAllowedError_WhenUserIsActive()
        {
            OperationResultDto<int> result = _userService.DeleteUser(ExistingId);
            
            Assert.Equal(ExistingId, result.Data);
            Assert.Equal(Results.NotAllowed, result.Result);
            Assert.Equal("Can't delete active user", result.Messages.First());
        }

        [Fact]
        [Unit("UserService")]
        public void Should_DeleteUser()
        {
            OperationResultDto<int> result = _userService.DeleteUser(InactiveId);

            Assert.Equal(InactiveId, result.Data);
            Assert.Equal(Results.Success, result.Result);
            Assert.Equal("User deleted with success", result.Messages.First());
        }
    }
}
