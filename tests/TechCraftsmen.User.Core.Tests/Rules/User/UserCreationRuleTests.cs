using Moq;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Rules.User;
using TechCraftsmen.User.Core.Utils;
using TechCraftsmen.User.Tests.Utils.Mock;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Rules.User
{
    public class UserCreationRuleTests
    {
        private readonly UserCreationRule _rule;
        private readonly UserGenerator _userGenerator;
        private readonly Mock<ICrudRepository<Entities.User>> _userRepository;

        private const string EXISTING_EMAIL = "exists@mail.com";
        private const string INEXISTING_EMAIL = "inexists@mail.com";

        public UserCreationRuleTests()
        {
            _userGenerator = new UserGenerator();
            _userRepository = new Mock<ICrudRepository<Entities.User>>();

            _userRepository.Setup(repo => repo.GetByFilter(FilterUtils.CreateDictionary("Email", EXISTING_EMAIL)))
                .Returns(() => new List<Entities.User>() { _userGenerator.Generate() }.AsQueryable());

            _userRepository.Setup(repo => repo.GetByFilter(FilterUtils.CreateDictionary("Email", INEXISTING_EMAIL)))
                .Returns(() => new List<Entities.User>().AsQueryable());

            _rule = new UserCreationRule(_userRepository.Object);
        }

        [Fact]
        [Unit("UserCreationRule")]
        public void Should_ReturnFalse_ForExistingEmail()
        {
            var result = _rule.Execute(EXISTING_EMAIL);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("E-mail already registered", result.Errors.FirstOrDefault());
        }

        [Theory]
        [Unit("UserCreationRule")]
        [InlineData("")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_ReturnFalse_ForPassword_NullOrEmpty(string email)
        {
            var result = _rule.Execute(email);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Parameter null or empty", result.Errors.FirstOrDefault());
        }

        [Fact]
        [Unit("UserCreationRule")]
        public void Should_ReturnTrue_ForInexistingEmail()
        {
            var result = _rule.Execute(INEXISTING_EMAIL);

            Assert.True(result.Success);
            Assert.False(result.Errors.Any());
        }
    }
}
