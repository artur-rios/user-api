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
using TechCraftsmen.User.Core.Services.Implementation;
using TechCraftsmen.User.Core.Validation;
using TechCraftsmen.User.Tests.Utils.Mock;
using Xunit;

namespace TechCraftsmen.User.Services.Tests
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;
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
        private const string EXISTING_EMAIL = "exists@mail.com";

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

            _userRepository.Setup(repo => repo.Update(_userMock));

            _userRepository.Setup(repo => repo.Delete(_userMock));

            _creationRule = new UserCreationRule(_userRepository.Object);

            _userFilterValidator = new UserFilterValidator();

            _userService = new UserService(_mapper, _userRepository.Object, _userValidator, _userFilterValidator, _creationRule, _updateRule, _statusUpdateRule, _deletionRule);
        }

        [Fact]
        public void Should_ThrowValidationException_When_UserDtoIsInvalid()
        {
            var invalidDto = _userDtoGenerator.WithEmail("").WithDefaultName().WithRandomPassword().Generate();

            Assert.Throws<ValidationException>(() => _userService.CreateUser(invalidDto));
        }

        [Fact]
        public void Should_ThrowNotAllowedException_When_CreationRuleFails()
        {
            var userWithExisitingEmail = _userDtoGenerator.WithEmail(EXISTING_EMAIL).WithDefaultName().WithRandomPassword().Generate();

            Assert.Throws<NotAllowedException>(() => _userService.CreateUser(userWithExisitingEmail));
        }

        [Fact]
        public void Should_CreateUser_And_ReturnId_ForValidDto()
        {
            var newId = _userService.CreateUser(_userDtoMock);

            Assert.Equal(_userMock.Id, newId);
        }

        [Fact]
        public void Should_ThrowNotFoundException_When_IdNotOnDatabase()
        {
            var exception = Assert.Throws<NotFoundException>(() => _userService.GetUserById(INEXISTING_ID));

            Assert.Equal("User not found!", exception.Message);
        }

        [Fact]
        public void Should_ReturnUser_When_IdIsOnDatabase()
        {
            var user = _userService.GetUserById(EXISTING_ID);

            Assert.NotNull(user);
            Assert.Equal(EXISTING_ID, user.Id);
        }
    }
}
