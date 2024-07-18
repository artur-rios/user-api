using Microsoft.AspNetCore.Http;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Rules.Role;

namespace TechCraftsmen.User.Core.Rules.User
{
    public class UserCreationRule(ICrudRepository<Entities.User> userRepository, IHttpContextAccessor httpContextAccessor, RoleRule roleRule) : BaseRule<Tuple<string, int>>
    {
        private readonly ICrudRepository<Entities.User> _userRepository = userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly RoleRule _roleRule = roleRule;

        public override RuleResultDto Execute(Tuple<string, int> parameter)
        {
            if (!IsParameterValid(parameter, out string validationMessage))
            {
                _result.Errors.Add(validationMessage);

                return Resolve();
            }

            parameter.Deconstruct(out string email, out int roleId);

            var filter = new Dictionary<string, object>()
            {
                {"Email", email }
            };

            var user = _userRepository.GetByFilter(filter);

            if (user.Any())
            {
                _result.Errors.Add("E-mail already registered");
            }

            if (roleId != (int)Roles.REGULAR)
            {
                _httpContextAccessor.HttpContext!.Items.TryGetValue("User", out var userData);

                var authenticatedUser = userData as UserDto;

                if (authenticatedUser!.RoleId != (int)Roles.ADMIN)
                {
                    _result.Errors.Add("Only admins can register this kind of user");
                }
            }

            return Resolve();
        }

        internal override bool IsParameterValid(Tuple<string, int> parameter, out string message)
        {
            parameter.Deconstruct(out string email, out int roleId);

            if (string.IsNullOrEmpty(email))
            {
                message = $"Email cannot be null or empty";

                return false;
            }

            var ruleResult = _roleRule.Execute(roleId);

            if (!ruleResult.Success)
            {
                message = $"RoleId must be valid";

                return false;
            }

            message = "Parameter valid";

            return true;
        }
    }
}
