using Microsoft.AspNetCore.Http;
using Moq;
using TechCraftsmen.User.Domain.Enums;
using TechCraftsmen.User.Domain.Interfaces;
using TechCraftsmen.User.Services.Dto;
using TechCraftsmen.User.Tests.Configuration.Attributes;
using TechCraftsmen.User.Tests.Mock.Data;
using TechCraftsmen.User.Tests.Mock.Generators;
using TechCraftsmen.User.Utils.Extensions;
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
            UserMockData userMocks = new();
            Mock<ICrudRepository<Domain.Entities.User>> userRepository = new();
            _httpContextAccessor = new Mock<HttpContextAccessor>();

            userRepository.Setup(repo => repo.GetByFilter(ExistingEmail.ToDictionary("Email"), false))
                .Returns(() => new List<Domain.Entities.User> { _userGenerator.WithDefaultEmail().WithDefaultName().WithRandomId().WithRandomPassword().Generate() }.AsQueryable());

            userRepository.Setup(repo => repo.GetByFilter(NonexistentEmail.ToDictionary("Email"), false))
                .Returns(() => new List<Domain.Entities.User>().AsQueryable());

            _httpContextAccessor.Object.HttpContext = new DefaultHttpContext();
            _httpContextAccessor.Object.HttpContext!.Items = new Dictionary<object, object?>() { { "User", userMocks.TestUserDto } };
        }
        
        [UnitFact("User")]
        public void Should_NotAllowCreationOfAdmins_IfAuthenticatedUserNotAnAdmin()
        {
            Domain.Entities.User user = _userGenerator.WithDefaultEmail().WithDefaultName().WithRandomId().WithRandomPassword()
                .WithRoleId((int)Roles.Admin).Generate();
                
            _httpContextAccessor.Object.HttpContext!.Items.TryGetValue("User", out object? userData);

            UserDto? authenticatedUser = userData as UserDto;

            bool canRegister = user.CanRegister(authenticatedUser!.RoleId, out string[] errors);

            Assert.False(canRegister);
            Assert.NotEmpty(errors);
            Assert.Equal("Only admins can register a user with Admin role", errors.FirstOrDefault());
        }
        
        [UnitFact("User")]
        public void Should_NotAllowDeletion_ForActiveUser()
        {
            Domain.Entities.User user = _userGenerator.WithStatus(true).Generate();

            bool canDelete = user.CanDelete(out string[] errors);

            Assert.False(canDelete);
            Assert.NotEmpty(errors);
            Assert.Equal("Can't delete active user", errors.FirstOrDefault());
        }
        
        [UnitFact("User")]
        public void Should_AllowDeletion_ForInactiveUser()
        {
            Domain.Entities.User user = _userGenerator.WithStatus(false).Generate();

            bool canDelete = user.CanDelete(out string[] errors);

            Assert.True(canDelete);
            Assert.Empty(errors);
        }
        
        [UnitFact("User")]
        public void Should_NotAllowActivation_ForActiveUser()
        {
            Domain.Entities.User user = _userGenerator.WithStatus(true).Generate();

            bool canActivate = user.CanActivate(out string[] errors);

            Assert.False(canActivate);
            Assert.NotEmpty(errors);
            Assert.Equal("User already active", errors.FirstOrDefault());
        }
        
        [UnitFact("User")]
        public void Should_AllowActivation_ForInactiveUser()
        {
            Domain.Entities.User user = _userGenerator.WithStatus(false).Generate();

            bool canActivate = user.CanActivate(out string[] errors);

            Assert.True(canActivate);
            Assert.Empty(errors);
        }
        
        [UnitFact("User")]
        public void Should_NotAllowDeactivation_ForInactiveUser()
        {
            Domain.Entities.User user = _userGenerator.WithStatus(false).Generate();

            bool canDeactivate = user.CanDeactivate(out string[] errors);

            Assert.False(canDeactivate);
            Assert.NotEmpty(errors);
            Assert.Equal("User already inactive", errors.FirstOrDefault());
        }
        
        [UnitFact("User")]
        public void Should_AllowDeactivation_ForInactiveUser()
        {
            Domain.Entities.User user = _userGenerator.WithStatus(true).Generate();

            bool canDeactivate = user.CanDeactivate(out string[] errors);

            Assert.True(canDeactivate);
            Assert.Empty(errors);
        }

        [UnitFact("User")]
        public void Should_NotUpdate_InactiveUser()
        {
            Domain.Entities.User user = _userGenerator.WithStatus(false).Generate();
            
            bool canUpdate = user.CanUpdate(out string[] errors);

            Assert.False(canUpdate);
            Assert.NotEmpty(errors);
            Assert.Equal("Can't update inactive user", errors.FirstOrDefault());
        }
        
        [UnitFact("User")]
        public void Should_Update_ActiveUser()
        {
            Domain.Entities.User user = _userGenerator.WithStatus(true).Generate();
            
            bool canUpdate = user.CanUpdate(out string[] errors);

            Assert.True(canUpdate);
            Assert.Empty(errors);
        }
    }
}
