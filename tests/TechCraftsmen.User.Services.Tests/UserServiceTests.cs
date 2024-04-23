using AutoMapper;
using FluentValidation;
using Moq;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Mapping;
using TechCraftsmen.User.Core.Rules.User;
using TechCraftsmen.User.Core.Services.Implementation;
using TechCraftsmen.User.Core.Utils;
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

        private readonly UserCreationRule _creationRule;
        private readonly UserUpdateRule _updateRule;
        private readonly UserStatusUpdateRule _statusUpdateRule;
        private readonly UserDeletionRule _deletionRule;

        private readonly UserGenerator _userGenerator = new();
        private readonly UserDtoGenerator _userDtoGenerator = new();

        private readonly Core.Entities.User _userMock;

        private const int EXISTING_ID = 1;
        private const int INEXISTING_ID = 2;
        private const string EXISTING_EMAIL = "exists@mail.com";
        private const string INEXISTING_EMAIL = "inexists@mail.com";
        private const string CORRECT_PASSWORD = "C0rrectPassw0rd";
        private const string INCORRECT_PASSWORD = "Inc0rrectPassw0rd";

        public UserServiceTests()
        {
            _mapper = new Mapper(AutoMapperConfiguration.UserMapping);
            _userRepository = new Mock<ICrudRepository<Core.Entities.User>>();
            _userValidator = new UserDtoValidator();
            _updateRule = new UserUpdateRule();
            _statusUpdateRule = new UserStatusUpdateRule();
            _deletionRule = new UserDeletionRule();

            _userMock = _userGenerator.WithDefaultEmail().WithDefaultName().WithRandomId().WithRandomPassword().Generate();

            _userRepository.Setup(repo => repo.Create(_userMock)).Returns(() => _userMock.Id);

            _userRepository.Setup(repo => repo.GetById(EXISTING_ID))
                .Returns(() => _userGenerator.WithId(EXISTING_ID).WithDefaultEmail().WithDefaultName().WithRandomPassword().Generate());

            _userRepository.Setup(repo => repo.GetById(EXISTING_ID)).Returns(() => null);

            _userRepository.Setup(repo => repo.GetByFilter(FilterUtils.CreateDictionary("Email", EXISTING_EMAIL)))
                .Returns(() => new List<Core.Entities.User>() { _userGenerator.WithId(EXISTING_ID).WithEmail(EXISTING_EMAIL).WithPassword(CORRECT_PASSWORD).Generate() }.AsQueryable());

            _userRepository.Setup(repo => repo.GetByFilter(FilterUtils.CreateDictionary("Email", INEXISTING_EMAIL)))
                .Returns(() => new List<Core.Entities.User>().AsQueryable());

            _userRepository.Setup(repo => repo.Update(_userMock));

            _userRepository.Setup(repo => repo.Delete(_userMock));

            _creationRule = new UserCreationRule(_userRepository.Object);

            _userService = new UserService(_mapper, _userRepository.Object, _userValidator, _creationRule, _updateRule, _statusUpdateRule, _deletionRule);
        }

        [Fact]
        public void Should_ThrowValidationException_When_UserDtoIsInvalid()
        {
            var invalidDto = _userDtoGenerator.WithEmail("").WithDefaultName().WithRandomPassword().Generate();

            Assert.Throws<ValidationException>(() => _userService.CreateUser(invalidDto));
        }
    }
}
