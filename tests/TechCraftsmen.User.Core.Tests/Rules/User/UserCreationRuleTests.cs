using Microsoft.AspNetCore.Http;
using Moq;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Rules.Role;
using TechCraftsmen.User.Core.Rules.User;
using TechCraftsmen.User.Core.Utils;
using TechCraftsmen.User.Tests.Utils.Generators;
using TechCraftsmen.User.Tests.Utils.Mock;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Rules.User
{
    public class UserCreationRuleTests
    {
        private readonly UserCreationRule _rule;
        private readonly UserGenerator _userGenerator;
        private readonly UserMocks _userMocks;
        private readonly Mock<ICrudRepository<Entities.User>> _userRepository;
        private readonly Mock<HttpContextAccessor> _httpContextAccessor;

        private const string EXISTING_EMAIL = "exists@mail.com";
        private const string INEXISTING_EMAIL = "inexists@mail.com";

        public UserCreationRuleTests()
        {
            _userGenerator = new UserGenerator();
            _userMocks = new UserMocks();
            _userRepository = new Mock<ICrudRepository<Entities.User>>();
            _httpContextAccessor = new Mock<HttpContextAccessor>();

            _userRepository.Setup(repo => repo.GetByFilter(FilterUtils.CreateDictionary("Email", EXISTING_EMAIL)))
                .Returns(() => new List<Entities.User>() { _userGenerator.WithDefaultEmail().WithDefaultName().WithRandomId().WithRandomPassword().Generate() }.AsQueryable());

            _userRepository.Setup(repo => repo.GetByFilter(FilterUtils.CreateDictionary("Email", INEXISTING_EMAIL)))
                .Returns(() => new List<Entities.User>().AsQueryable());

            _httpContextAccessor.Object.HttpContext = new DefaultHttpContext();
            _httpContextAccessor.Object.HttpContext!.Items = new Dictionary<object, object?>() { { "User", _userMocks.TestUserDto } };

            _rule = new UserCreationRule(_userRepository.Object, _httpContextAccessor.Object, new RoleRule());
        }

        [Fact]
        [Unit("UserCreationRule")]
        public void Should_ReturnFalse_ForExistingEmail()
        {
            var result = _rule.Execute(new Tuple<string, int>(EXISTING_EMAIL, 3));

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("E-mail already registered", result.Errors.FirstOrDefault());
        }

        [Theory]
        [Unit("UserCreationRule")]
        [InlineData("")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_ReturnFalse_ForEmail_NullOrEmpty(string email)
        {
            var result = _rule.Execute(new Tuple<string, int>(email, 2));

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Email cannot be null or empty", result.Errors.FirstOrDefault());
        }

        [Fact]
        [Unit("UserCreationRule")]
        public void Should_ReturnTrue_ForInexistingEmail()
        {
            var result = _rule.Execute(new Tuple<string, int>(INEXISTING_EMAIL, 2));

            Assert.True(result.Success);
            Assert.False(result.Errors.Any());
        }

        [Fact]
        [Unit("UserCreationRule")]
        public void Should_NotAllowCreationOfAdmins_IfAuthenticatedUserNotAnAdmin()
        {
            var result = _rule.Execute(new Tuple<string, int>(INEXISTING_EMAIL, 1));

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Only admins can register this kind of user", result.Errors.FirstOrDefault());
        }
    }
}
