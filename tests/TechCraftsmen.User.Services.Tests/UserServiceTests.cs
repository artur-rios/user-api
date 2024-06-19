using AutoMapper;
using FluentValidation;
using Moq;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Filters;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Mapping;
using TechCraftsmen.User.Core.Rules.Password;
using TechCraftsmen.User.Core.Rules.User;
using TechCraftsmen.User.Core.Utils;
using TechCraftsmen.User.Core.Validation;
using TechCraftsmen.User.Tests.Utils.Mock;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Services.Tests
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mapper _mapper;
        private readonly Mock<ICrudRepository<Core.Entities.User>> _userRepository;
        private readonly IValidator<UserDto> _userValidator;
        private readonly UserFilterValidator _userFilterValidator;
        private readonly UserCreationRule _creationRule;
        private readonly UserUpdateRule _updateRule;
        private readonly UserStatusUpdateRule _statusUpdateRule;
        private readonly UserDeletionRule _deletionRule;

        private readonly RandomStringGenerator _randomStringGenerator = new();
        private readonly UserGenerator _userGenerator = new();
        private readonly UserDtoGenerator _userDtoGenerator = new();

        private readonly string _passwordMock;
        private readonly UserDto _userDtoMock;
        private readonly Core.Entities.User _userMock;

        private const int EXISTING_ID = 1;
        private const int INEXISTING_ID = 2;
        private const int INACTIVE_ID = 3;
        private const string EXISTING_EMAIL = "exists@mail.com";
        private const string INEXISTING_EMAIL = "inexists@mail.com";

        public UserServiceTests()
        {
            _mapper = new Mapper(AutoMapperConfiguration.UserMapping);
            _userRepository = new Mock<ICrudRepository<Core.Entities.User>>();
            _userValidator = new UserDtoValidator();
            _updateRule = new UserUpdateRule();
            _statusUpdateRule = new UserStatusUpdateRule();
            _deletionRule = new UserDeletionRule();

            _passwordMock = _randomStringGenerator.WithLength(PasswordRule.MINIMUM_LENGTH).WithNumbers().WithLowerChars().WithUpperChars().Generate();

            _userMock = _userGenerator.WithDefaultEmail().WithDefaultName().WithRandomId().WithPassword(_passwordMock).Generate();
            _userDtoMock = _mapper.Map<UserDto>(_userMock);
            _userDtoMock.Password = _passwordMock;

            _userRepository.Setup(repo => repo.Create(It.IsAny<Core.Entities.User>())).Returns(() => _userMock.Id);

            _userRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    if (id == EXISTING_ID)
                    {
                        return _userGenerator.WithId(EXISTING_ID).WithDefaultEmail().WithDefaultName().WithRandomPassword().Generate();
                    }

                    if (id == INACTIVE_ID)
                    {
                        return _userGenerator.WithId(INACTIVE_ID).WithDefaultEmail().WithDefaultName().WithRandomPassword().WithStatus(false).Generate();
                    }

                    return null;
                });

            _userRepository.Setup(repo => repo.GetByFilter(It.IsAny<Dictionary<string, object>>()))
                .Returns((Dictionary<string, object> filter) =>
                {
                    if (filter.FirstOrDefault().Value as string == EXISTING_EMAIL)
                    {
                        return new List<Core.Entities.User>() { _userGenerator.WithId(EXISTING_ID).WithEmail(EXISTING_EMAIL).WithRandomPassword().Generate() }.AsQueryable();
                    }

                    return new List<Core.Entities.User>().AsQueryable();
                });

            _userRepository.Setup(repo => repo.Update(It.IsAny<Core.Entities.User>()));

            _userRepository.Setup(repo => repo.Delete(It.IsAny<Core.Entities.User>()));

            _creationRule = new UserCreationRule(_userRepository.Object);

            _userFilterValidator = new UserFilterValidator();

            _userService = new UserService(_mapper, _userRepository.Object, _userValidator, _userFilterValidator, _creationRule, _updateRule, _statusUpdateRule, _deletionRule);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowValidationException_When_UserDtoIsInvalid()
        {
            var invalidDto = _userDtoGenerator.WithEmail("").WithDefaultName().WithRandomPassword().Generate();

            Assert.Throws<ValidationException>(() => _userService.CreateUser(invalidDto));
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotAllowedException_When_CreationRuleFails()
        {
            var userWithExisitingEmail = _userDtoGenerator.WithEmail(EXISTING_EMAIL).WithDefaultName().WithRandomPassword().Generate();

            Assert.Throws<NotAllowedException>(() => _userService.CreateUser(userWithExisitingEmail));
        }

        [Fact]
        [Unit("UserService")]
        public void Should_CreateUser_And_ReturnId_ForValidDto()
        {
            var newId = _userService.CreateUser(_userDtoMock);

            Assert.Equal(_userMock.Id, newId);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotFoundException_When_IdNotOnDatabase()
        {
            var exception = Assert.Throws<NotFoundException>(() => _userService.GetUserById(INEXISTING_ID));

            Assert.Equal("User not found!", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnUser_When_IdIsOnDatabase()
        {
            var user = _userService.GetUserById(EXISTING_ID);

            Assert.NotNull(user);
            Assert.Equal(EXISTING_ID, user.Id);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotAllowedException_When_FilterIsInvalid()
        {
            var exception = Assert.Throws<NotAllowedException>(() => _userService.GetUsersByFilter(FilterUtils.CreateQuery("InvalidFilter", "xxxxx")));

            Assert.Equal("No valid filters were passed!", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotFoundException_When_FilterDoesNotReturnAnyResult()
        {
            var exception = Assert.Throws<NotFoundException>(() => _userService.GetUsersByFilter(FilterUtils.CreateQuery("Email", INEXISTING_EMAIL)));

            Assert.Equal("No users found with the given filter", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnUsers_For_ValidAndExistingFilter()
        {
            var users = _userService.GetUsersByFilter(FilterUtils.CreateQuery("Email", EXISTING_EMAIL));

            Assert.NotNull(users);
            Assert.NotEmpty(users);
            Assert.True(users.FirstOrDefault(user => user.Email == EXISTING_EMAIL) is not null);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotFoundException_WhenIdNotOnDatabaseAndPasswordCannotBeFound()
        {
            var exception = Assert.Throws<NotFoundException>(() => _userService.GetPasswordByUserId(INEXISTING_ID));

            Assert.Equal("User not found!", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ReturnPassword_When_IdIsOnDatabase()
        {
            var hashDto = _userService.GetPasswordByUserId(EXISTING_ID);

            Assert.NotNull(hashDto);
            Assert.NotEmpty(hashDto.Hash);
            Assert.NotEmpty(hashDto.Salt);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotFoundException_WhenIdNotOnDatabaseAndCannotUpdateUser()
        {
            var userMock = _userGenerator.WithDefaultEmail().WithDefaultName().WithId(INEXISTING_ID).WithPassword(_passwordMock).Generate();
            var userDto = _mapper.Map<UserDto>(userMock);

            var exception = Assert.Throws<NotFoundException>(() => _userService.UpdateUser(userDto));

            Assert.Equal("User not found!", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotNotAllowedException_WhenUserIsInactive()
        {
            var userMock = _userGenerator.WithDefaultEmail().WithDefaultName().WithId(INACTIVE_ID).WithPassword(_passwordMock).Generate();
            var userDto = _mapper.Map<UserDto>(userMock);

            var exception = Assert.Throws<NotAllowedException>(() => _userService.UpdateUser(userDto));

            Assert.Equal("Can't update inactive user", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_UpdateUser()
        {
            var userMock = _userGenerator.WithDefaultEmail().WithDefaultName().WithId(EXISTING_ID).WithPassword(_passwordMock).Generate();
            var userDto = _mapper.Map<UserDto>(userMock);

            var exception = Record.Exception(() => _userService.UpdateUser(userDto));

            Assert.Null(exception);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotFoundException_WhenIdNotOnDatabaseAndCannotActivateUser()
        {
            var exception = Assert.Throws<NotFoundException>(() => _userService.ActivateUser(INEXISTING_ID));

            Assert.Equal("User not found!", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotNotAllowedException_WhenUserIsActive()
        {
            var exception = Assert.Throws<EntityNotChangedException>(() => _userService.ActivateUser(EXISTING_ID));

            Assert.Equal("User already active", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ActivateUser()
        {
            var exception = Record.Exception(() => _userService.ActivateUser(INACTIVE_ID));

            Assert.Null(exception);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotFoundException_WhenIdNotOnDatabaseAndCannotDeactivateUser()
        {
            var exception = Assert.Throws<NotFoundException>(() => _userService.DeactivateUser(INEXISTING_ID));

            Assert.Equal("User not found!", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotNotAllowedException_WhenUserIsInactive_On_Deactivation()
        {
            var exception = Assert.Throws<EntityNotChangedException>(() => _userService.DeactivateUser(INACTIVE_ID));

            Assert.Equal("User already inactive", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_DeactivateUser()
        {
            var exception = Record.Exception(() => _userService.DeactivateUser(EXISTING_ID));

            Assert.Null(exception);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotFoundException_WhenIdNotOnDatabaseAndCannotDeleteUser()
        {
            var exception = Assert.Throws<NotFoundException>(() => _userService.DeleteUser(INEXISTING_ID));

            Assert.Equal("User not found!", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_ThrowNotAllowedException_WhenIdNotOnDatabaseAndCannotDeleteUser()
        {
            var exception = Assert.Throws<NotAllowedException>(() => _userService.DeleteUser(EXISTING_ID));

            Assert.Equal("Can't delete active user", exception.Message);
        }

        [Fact]
        [Unit("UserService")]
        public void Should_DeleteUser()
        {
            var exception = Record.Exception(() => _userService.DeleteUser(INACTIVE_ID));

            Assert.Null(exception);
        }
    }
}
