using FluentValidation.TestHelper;
using TechCraftsmen.User.Services.Dto;
using TechCraftsmen.User.Services.Validation;
using TechCraftsmen.User.Tests.Configuration.Attributes;
using TechCraftsmen.User.Tests.Mock.Generators;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Validation.Fluent
{
    public class UserDtoValidatorTests
    {
        private readonly UserDtoGenerator _userDtoGenerator = new();
        private readonly UserDtoValidator _validator = new();
        
        [UnitTheory("UserDtoValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_HaveError_ForNameNullOrEmpty(string name)
        {
            UserDto user = _userDtoGenerator.WithDefaultEmail().WithRandomPassword().WithName(name).WithRoleId().Generate();

            TestValidationResult<UserDto>? result = _validator.TestValidate(user);

            result.ShouldHaveValidationErrorFor("Name");
            result.ShouldNotHaveValidationErrorFor("Email");
            result.ShouldNotHaveValidationErrorFor("Password");
            result.ShouldNotHaveValidationErrorFor("RoleId");
        }
        
        [UnitTheory("UserDtoValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("ma@jjf..com")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_HaveError_ForInvalidEmail(string email)
        {
            UserDto user = _userDtoGenerator.WithDefaultName().WithRandomPassword().WithEmail(email).WithRoleId().Generate();

            TestValidationResult<UserDto>? result = _validator.TestValidate(user);

            result.ShouldNotHaveValidationErrorFor("Name");
            result.ShouldHaveValidationErrorFor("Email");
            result.ShouldNotHaveValidationErrorFor("Password");
            result.ShouldNotHaveValidationErrorFor("RoleId");
        }
        
        [UnitTheory("UserDtoValidator")]
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
            UserDto user = _userDtoGenerator.WithDefaultName().WithDefaultEmail().WithPassword(password).WithRoleId().Generate();

            TestValidationResult<UserDto>? result = _validator.TestValidate(user);

            result.ShouldNotHaveValidationErrorFor("Name");
            result.ShouldNotHaveValidationErrorFor("Email");
            result.ShouldHaveValidationErrorFor("Password");
            result.ShouldNotHaveValidationErrorFor("RoleId");
        }
        
        [UnitFact("UserDtoValidator")]
        public void Should_NotHaveError_ForValidUserDto()
        {
            UserDto user = _userDtoGenerator.WithDefaultName().WithDefaultEmail().WithRandomPassword().WithRoleId().Generate();

            TestValidationResult<UserDto>? result = _validator.TestValidate(user);

            result.ShouldNotHaveValidationErrorFor("Name");
            result.ShouldNotHaveValidationErrorFor("Email");
            result.ShouldNotHaveValidationErrorFor("Password");
            result.ShouldNotHaveValidationErrorFor("RoleId");
        }
    }
}
