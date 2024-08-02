using FluentValidation;
using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Core.Validation.Fluent
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        private readonly EmailValidator _emailRule = new();
        private readonly PasswordValidator _passwordValidator = new();
        private readonly RoleIdValidator _roleIdValidator = new();

        public UserDtoValidator()
        {
            RuleFor(user => user.Name).NotEmpty();
            RuleFor(user => user.Email).NotEmpty();
            RuleFor(user => user.Email).Custom((email, context) =>
            {
                var ruleResult = _emailRule.Validate(email);

                if (!ruleResult.Success)
                {
                    foreach (var error in ruleResult.Errors)
                    {
                        context.AddFailure(error);
                    }
                }
            });
            RuleFor(user => user.Password).NotEmpty();
            RuleFor(user => user.Password).Custom((password, context) =>
            {
                var ruleResult = _passwordValidator.Validate(password);

                if (!ruleResult.Success)
                {
                    foreach (var error in ruleResult.Errors)
                    {
                        context.AddFailure(error);
                    }
                }
            });
            RuleFor(user => user.RoleId).Custom((roleId, context) =>
            {
                var ruleResult = _roleIdValidator.Validate(roleId);

                if (!ruleResult.Success)
                {
                    foreach (var error in ruleResult.Errors)
                    {
                        context.AddFailure(error);
                    }
                }
            });
        }
    }
}
