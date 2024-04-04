﻿using FluentValidation;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Rules.Email;
using TechCraftsmen.User.Core.Rules.Password;

namespace TechCraftsmen.User.Core.Validation
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        private readonly EmailRule _emailRule = new();
        private readonly PasswordRule _passwordRule = new();

        public UserDtoValidator()
        {
            RuleFor(user => user.Name).NotEmpty();
            RuleFor(user => user.Email).NotEmpty();
            RuleFor(user => user.Email).Custom((email, context) =>
            {
                var ruleResult = _emailRule.Execute(email);

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
                var ruleResult = _passwordRule.Execute(password);

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
