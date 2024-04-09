using FluentValidation.TestHelper;
using TechCraftsmen.User.Core.Validation;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Validation
{
    public class UserDtoValidatorTests
    {
        private readonly UserDtoValidator _validator = new();

        [Theory]
        [Unit("UserDtoValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_HaveError_ForNameNullOrEmpty(string name)
        {
            var credentialsDto = new Dto.UserDto() { Name = name, Email = "test@mail.com", Password = "1A2b#cd#ef", RoleId = 2 };

            var result = _validator.TestValidate(credentialsDto);

            result.ShouldHaveValidationErrorFor(user => user.Name);
            result.ShouldNotHaveValidationErrorFor(user => user.Email);
            result.ShouldNotHaveValidationErrorFor(user => user.Password);
            result.ShouldNotHaveValidationErrorFor(user => user.RoleId);
        }

        [Theory]
        [Unit("UserDtoValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("ma@jjf..com")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_HaveError_ForInvalidEmail(string email)
        {
            var credentialsDto = new Dto.UserDto() { Name = "Jhon Doe", Email = email, Password = "1A2b#cd#ef", RoleId = 2 };

            var result = _validator.TestValidate(credentialsDto);

            result.ShouldNotHaveValidationErrorFor(user => user.Name);
            result.ShouldHaveValidationErrorFor(user => user.Email);
            result.ShouldNotHaveValidationErrorFor(user => user.Password);
            result.ShouldNotHaveValidationErrorFor(user => user.RoleId);
        }

        [Theory]
        [Unit("UserDtoValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Ab#cd#ef#")]
        [InlineData("1A2B#CD#EF")]
        [InlineData("1a2b#cd#ef")]
        [InlineData("1A2b#3c")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_HaveError_ForInvalidPassword(string password)
        {
            var credentialsDto = new Dto.UserDto() { Name = "Jhon Doe", Email = "test@mail.com", Password = password, RoleId = 2 };

            var result = _validator.TestValidate(credentialsDto);

            result.ShouldNotHaveValidationErrorFor(user => user.Name);
            result.ShouldNotHaveValidationErrorFor(user => user.Email);
            result.ShouldHaveValidationErrorFor(user => user.Password);
            result.ShouldNotHaveValidationErrorFor(user => user.RoleId);
        }

        [Fact]
        [Unit("UserDtoValidator")]
        public void Should_NotHaveError_ForValidUserDto()
        {
            var credentialsDto = new Dto.UserDto() { Name = "Jhon Doe", Email = "test@mail.com", Password = "1A2b#cd#ef", RoleId = 2 };

            var result = _validator.TestValidate(credentialsDto);

            result.ShouldNotHaveValidationErrorFor(user => user.Name);
            result.ShouldNotHaveValidationErrorFor(user => user.Email);
            result.ShouldNotHaveValidationErrorFor(user => user.Password);
            result.ShouldNotHaveValidationErrorFor(user => user.RoleId);
        }
    }
}
