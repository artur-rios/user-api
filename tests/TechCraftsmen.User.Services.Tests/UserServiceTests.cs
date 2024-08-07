using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Moq;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Filters;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Mapping;
using TechCraftsmen.User.Core.Validation;
using TechCraftsmen.User.Core.Validation.Fluent;
using TechCraftsmen.User.Tests.Utils.Generators;
using TechCraftsmen.User.Tests.Utils.Mock;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Services.Tests
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mapper _mapper;

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
            _mapper = new Mapper(AutoMapperConfiguration.UserMapping);
            Mock<ICrudRepository<Core.Entities.User>> userRepository = new();
            IValidator<UserDto> userValidator = new UserDtoValidator();
            Mock<HttpContextAccessor> httpContextAccessor = new();

            _passwordMock = _randomStringGenerator.WithLength(PasswordValidator.MINIMUM_LENGTH).WithNumbers()
                .WithLowerChars().WithUpperChars().Generate();

            _userMock = _userGenerator.WithDefaultEmail().WithDefaultName().WithRandomId().WithPassword(_passwordMock)
                .WithRoleId((int)Roles.Regular).Generate();
            _userDtoMock = _mapper.Map<UserDto>(_userMock);
            _userDtoMock.Password = _passwordMock;
            UserMocks userMocks = new();

            userRepository.Setup(repo => repo.Create(It.IsAny<Core.Entities.User>())).Returns(() => _userMock.Id);

            userRepository.Setup(repo => repo.GetByFilter(It.IsAny<Dictionary<string, object>>()))
                .Returns((Dictionary<string, object> filter) =>
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

            _userService = new UserService(userRepository.Object, httpContextAccessor.Object, _mapper, userValidator);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowValidationException_When_UserDtoIsInvalid()
        {
            UserDto invalidDto = _userDtoGenerator.WithEmail("").WithDefaultName().WithRandomPassword().Generate();

            Assert.Throws<ValidationException>(() => _userService.CreateUser(invalidDto));
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotAllowedException_When_CreationRuleFails()
        {
            UserDto userWithNonexistentEmail = _userDtoGenerator.WithEmail(ExistingEmail).WithDefaultName()
                .WithRandomPassword().WithRoleId().Generate();

            Assert.Throws<NotAllowedException>(() => _userService.CreateUser(userWithNonexistentEmail));
        }

        [Fact]
        [Unit("UserService")]
        public void Should_CreateUser_And_ReturnId_ForValidDto()
        {
            int newId = _userService.CreateUser(_userDtoMock);

            Assert.Equal(_userMock.Id, newId);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotFoundException_When_IdNotOnDatabase()
        {
            NotFoundException exception =
                Assert.Throws<NotFoundException>(() => _userService.GetUsersByFilter(new UserFilter(NonexistentId)));

            Assert.Equal("No users found with the given filter", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnUser_When_IdIsOnDatabase()
        {
            UserDto? user = _userService.GetUsersByFilter(new UserFilter(ExistingId)).FirstOrDefault();

            Assert.NotNull(user);
            Assert.Equal(ExistingId, user.Id);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotFoundException_When_FilterDoesNotReturnAnyResult()
        {
            NotFoundException exception =
                Assert.Throws<NotFoundException>(() => _userService.GetUsersByFilter(new UserFilter(NonexistentEmail)));

            Assert.Equal("No users found with the given filter", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnUsers_For_ValidAndExistingFilter()
        {
            IList<UserDto> users = _userService.GetUsersByFilter(new UserFilter(ExistingEmail));

            Assert.NotNull(users);
            Assert.NotEmpty(users);
            Assert.True(users.FirstOrDefault(user => user.Email == ExistingEmail) is not null);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotFoundException_WhenIdNotOnDatabaseAndPasswordCannotBeFound()
        {
            NotFoundException exception =
                Assert.Throws<NotFoundException>(() => _userService.GetPasswordByUserId(NonexistentId));

            Assert.Equal("User not found!", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnPassword_When_IdIsOnDatabase()
        {
            HashDto hashDto = _userService.GetPasswordByUserId(ExistingId);

            Assert.NotNull(hashDto);
            Assert.NotEmpty(hashDto.Hash);
            Assert.NotEmpty(hashDto.Salt);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotFoundException_WhenIdNotOnDatabaseAndCannotUpdateUser()
        {
            Core.Entities.User userMock = _userGenerator.WithDefaultEmail().WithDefaultName().WithId(NonexistentId)
                .WithPassword(_passwordMock).Generate();
            UserDto? userDto = _mapper.Map<UserDto>(userMock);

            NotFoundException exception = Assert.Throws<NotFoundException>(() => _userService.UpdateUser(userDto));

            Assert.Equal("User not found!", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotNotAllowedException_WhenUserIsInactive()
        {
            Core.Entities.User userMock = _userGenerator.WithDefaultEmail().WithDefaultName().WithId(InactiveId)
                .WithPassword(_passwordMock).Generate();
            UserDto? userDto = _mapper.Map<UserDto>(userMock);

            NotAllowedException exception = Assert.Throws<NotAllowedException>(() => _userService.UpdateUser(userDto));

            Assert.Equal("Can't update inactive user", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_UpdateUser()
        {
            Core.Entities.User userMock = _userGenerator.WithDefaultEmail().WithDefaultName().WithId(ExistingId)
                .WithPassword(_passwordMock).Generate();
            UserDto? userDto = _mapper.Map<UserDto>(userMock);

            Exception? exception = Record.Exception(() => _userService.UpdateUser(userDto));

            Assert.Null(exception);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotFoundException_WhenIdNotOnDatabaseAndCannotActivateUser()
        {
            NotFoundException exception =
                Assert.Throws<NotFoundException>(() => _userService.ActivateUser(NonexistentId));

            Assert.Equal("User not found!", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotNotAllowedException_WhenUserIsActive()
        {
            EntityNotChangedException exception =
                Assert.Throws<EntityNotChangedException>(() => _userService.ActivateUser(ExistingId));

            Assert.Equal("User already active", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ActivateUser()
        {
            Exception? exception = Record.Exception(() => _userService.ActivateUser(InactiveId));

            Assert.Null(exception);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotFoundException_WhenIdNotOnDatabaseAndCannotDeactivateUser()
        {
            NotFoundException exception =
                Assert.Throws<NotFoundException>(() => _userService.DeactivateUser(NonexistentId));

            Assert.Equal("User not found!", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotNotAllowedException_WhenUserIsInactive_On_Deactivation()
        {
            EntityNotChangedException exception =
                Assert.Throws<EntityNotChangedException>(() => _userService.DeactivateUser(InactiveId));

            Assert.Equal("User already inactive", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_DeactivateUser()
        {
            Exception? exception = Record.Exception(() => _userService.DeactivateUser(ExistingId));

            Assert.Null(exception);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotFoundException_WhenIdNotOnDatabaseAndCannotDeleteUser()
        {
            NotFoundException exception =
                Assert.Throws<NotFoundException>(() => _userService.DeleteUser(NonexistentId));

            Assert.Equal("User not found!", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotAllowedException_WhenIdNotOnDatabaseAndCannotDeleteUser()
        {
            NotAllowedException exception =
                Assert.Throws<NotAllowedException>(() => _userService.DeleteUser(ExistingId));

            Assert.Equal("Can't delete active user", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_DeleteUser()
        {
            Exception? exception = Record.Exception(() => _userService.DeleteUser(InactiveId));

            Assert.Null(exception);
        }
    }
}