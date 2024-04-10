using FluentValidation.TestHelper;
using TechCraftsmen.User.Core.Validation;
using TechCraftsmen.User.Tests.Utils.Mock;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Validation
{
    public class UserDtoValidatorTests
    {
        private readonly UserDtoGenerator _userDtoGenerator = new();
        private readonly UserDtoValidator _validator = new();

        [Theory]
        [Unit("UserDtoValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_HaveError_ForNameNullOrEmpty(string name)
        {
            var user = _userDtoGenerator.WithDefaultEmail().WithRandomPassword().WithName(name).Generate();

            var result = _validator.TestValidate(user);

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
            var user = _userDtoGenerator.WithDefaultName().WithRandomPassword().WithEmail(email).Generate();

            var result = _validator.TestValidate(user);

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
            var user = _userDtoGenerator.WithDefaultName().WithDefaultEmail().WithPassword(password).Generate();

            var result = _validator.TestValidate(user);

            result.ShouldNotHaveValidationErrorFor(user => user.Name);
            result.ShouldNotHaveValidationErrorFor(user => user.Email);
            result.ShouldHaveValidationErrorFor(user => user.Password);
            result.ShouldNotHaveValidationErrorFor(user => user.RoleId);
        }

        [Fact]
        [Unit("UserDtoValidator")]
        public void Should_NotHaveError_ForValidUserDto()
        {
            var user = _userDtoGenerator.WithDefaultName().WithDefaultEmail().WithRandomPassword().Generate();

            var result = _validator.TestValidate(user);

            result.ShouldNotHaveValidationErrorFor(user => user.Name);
            result.ShouldNotHaveValidationErrorFor(user => user.Email);
            result.ShouldNotHaveValidationErrorFor(user => user.Password);
            result.ShouldNotHaveValidationErrorFor(user => user.RoleId);
        }
    }
}
