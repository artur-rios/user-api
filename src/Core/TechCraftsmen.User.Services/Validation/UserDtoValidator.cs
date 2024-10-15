using FluentValidation;
using TechCraftsmen.User.Core.Validation;
using TechCraftsmen.User.Core.ValueObjects;
using TechCraftsmen.User.Services.Dto;

namespace TechCraftsmen.User.Services.Validation
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        private readonly EmailValidator _emailRule = new();
        private readonly PasswordValidator _passwordValidator = new();
        private readonly RoleIdValidator _roleIdValidator = new();

        public UserDtoValidator()
        {
            RuleFor(user => user.Name).NotEmpty();
            RuleFor(user => user.Email).Custom((email, context) =>
            {
                DomainOutput ruleResult = _emailRule.Validate(email);

                if (ruleResult.Success)
                {
                    return;
                }

                foreach (string? error in ruleResult.Errors)
                {
                    context.AddFailure(error);
                }
            });
            RuleFor(user => user.Password).NotEmpty();
            RuleFor(user => user.Password).Custom((password, context) =>
            {
                DomainOutput ruleResult = _passwordValidator.Validate(password);

                if (ruleResult.Success)
                {
                    return;
                }

                foreach (string? error in ruleResult.Errors)
                {
                    context.AddFailure(error);
                }
            });
            RuleFor(user => user.RoleId).Custom((roleId, context) =>
            {
                DomainOutput ruleResult = _roleIdValidator.Validate(roleId);

                if (ruleResult.Success)
                {
                    return;
                }

                foreach (string? error in ruleResult.Errors)
                {
                    context.AddFailure(error);
                }
            });
        }
    }
}
