using Microsoft.AspNetCore.Http;
using Moq;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Utils;
using TechCraftsmen.User.Tests.Utils.Generators;
using TechCraftsmen.User.Tests.Utils.Mock;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Entities
{
    public class UserTests
    {
        private readonly UserGenerator _userGenerator;
        private readonly Mock<HttpContextAccessor> _httpContextAccessor;
        
        private const string ExistingEmail = "exists@mail.com";
        private const string NonexistentEmail = "nonexists@mail.com";

        public UserTests()
        {
            _userGenerator = new UserGenerator();
            UserMocks userMocks = new();
            Mock<ICrudRepository<Core.Entities.User>> userRepository = new();
            _httpContextAccessor = new Mock<HttpContextAccessor>();

            userRepository.Setup(repo => repo.GetByFilter(FilterUtils.CreateDictionary("Email", ExistingEmail)))
                .Returns(() => new List<Core.Entities.User>() { _userGenerator.WithDefaultEmail().WithDefaultName().WithRandomId().WithRandomPassword().Generate() }.AsQueryable());

            userRepository.Setup(repo => repo.GetByFilter(FilterUtils.CreateDictionary("Email", NonexistentEmail)))
                .Returns(() => new List<Core.Entities.User>().AsQueryable());

            _httpContextAccessor.Object.HttpContext = new DefaultHttpContext();
            _httpContextAccessor.Object.HttpContext!.Items = new Dictionary<object, object?>() { { "User", userMocks.TestUserDto } };
        }
        
        [Fact]
        [Unit("User")]
        public void Should_NotAllowCreationOfAdmins_IfAuthenticatedUserNotAnAdmin()
        {
            var user = _userGenerator.WithDefaultEmail().WithDefaultName().WithRandomId().WithRandomPassword()
                .WithRoleId((int)Roles.ADMIN).Generate();
                
            _httpContextAccessor.Object.HttpContext!.Items.TryGetValue("User", out var userData);

            var authenticatedUser = userData as UserDto;

            var canRegister = user.CanRegister(authenticatedUser!.RoleId);

            Assert.False(canRegister.Success);
            Assert.True(canRegister.Errors.Any());
            Assert.Equal("Only admins can register this kind of user", canRegister.Errors.FirstOrDefault());
        }
        
        [Fact]
        [Unit("User")]
        public void Should_NotAllowDeletion_ForActiveUser()
        {
            var user = _userGenerator.WithStatus(true).Generate();

            var canDelete = user.CanDelete();

            Assert.False(canDelete.Success);
            Assert.True(canDelete.Errors.Any());
            Assert.Equal("Can't delete active user", canDelete.Errors.FirstOrDefault());
        }

        [Fact]
        [Unit("User")]
        public void Should_AllowDeletion_ForInactiveUser()
        {
            var user = _userGenerator.WithStatus(false).Generate();

            var canDelete = user.CanDelete();

            Assert.True(canDelete.Success);
            Assert.False(canDelete.Errors.Any());
        }
        
        [Fact]
        [Unit("User")]
        public void Should_NotAllowActivation_ForActiveUser()
        {
            var user = _userGenerator.WithStatus(true).Generate();

            var canActivate = user.CanActivate();

            Assert.False(canActivate.Success);
            Assert.True(canActivate.Errors.Any());
            Assert.Equal($"User already active", canActivate.Errors.FirstOrDefault());
        }
        
        [Fact]
        [Unit("User")]
        public void Should_AllowActivation_ForInactiveUser()
        {
            var user = _userGenerator.WithStatus(false).Generate();

            var canActivate = user.CanActivate();

            Assert.True(canActivate.Success);
            Assert.False(canActivate.Errors.Any());
        }

        [Fact]
        [Unit("User")]
        public void Should_NotAllowDeactivation_ForInactiveUser()
        {
            var user = _userGenerator.WithStatus(false).Generate();

            var canDeactivate = user.CanDeactivate();

            Assert.False(canDeactivate.Success);
            Assert.True(canDeactivate.Errors.Any());
            Assert.Equal($"User already inactive", canDeactivate.Errors.FirstOrDefault());
        }
        
        [Fact]
        [Unit("User")]
        public void Should_AllowDeactivation_ForInactiveUser()
        {
            var user = _userGenerator.WithStatus(true).Generate();

            var canDeactivate = user.CanDeactivate();

            Assert.True(canDeactivate.Success);
            Assert.False(canDeactivate.Errors.Any());
        }
        
        [Fact]
        [Unit("User")]
        public void Should_NotUpdate_InactiveUser()
        {
            var user = _userGenerator.WithStatus(false).Generate();
            
            var canUpdate = user.CanUpdate();

            Assert.False(canUpdate.Success);
            Assert.True(canUpdate.Errors.Any());
            Assert.Equal("Can't update inactive user", canUpdate.Errors.FirstOrDefault());
        }

        [Fact]
        [Unit("User")]
        public void Should_Update_ActiveUser()
        {
            var user = _userGenerator.WithStatus(true).Generate();
            
            var canUpdate = user.CanUpdate();

            Assert.True(canUpdate.Success);
            Assert.False(canUpdate.Errors.Any());
        }
    }
}
