using FluentValidation;
using TechCraftsmen.User.Domain.Output;
using TechCraftsmen.User.Domain.Validation;
using TechCraftsmen.User.Services.Dto;

namespace TechCraftsmen.User.Services.Validation
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        private readonly EmailValidator _emailValidator = new();
        private readonly PasswordValidator _passwordValidator = new();
        private readonly RoleIdValidator _roleIdValidator = new();

        public UserDtoValidator()
        {
            RuleFor(user => user.Name).NotEmpty();
            RuleFor(user => user.Email).Custom((email, context) =>
            {
                ProcessOutput validationResult = _emailValidator.Validate(email);

                if (validationResult.Success)
                {
                    return;
                }

                foreach (string? error in validationResult.Errors)
                {
                    context.AddFailure(error);
                }
            });
            RuleFor(user => user.Password).NotEmpty();
            RuleFor(user => user.Password).Custom((password, context) =>
            {
                ProcessOutput ruleResult = _passwordValidator.Validate(password);

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
                ProcessOutput ruleResult = _roleIdValidator.Validate(roleId);

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
