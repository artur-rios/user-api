using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Interfaces.Repositories;

namespace TechCraftsmen.User.Core.Rules.User
{
    public class UserCreationRule : BaseRule<string>
    {
        private readonly ICrudRepository<Entities.User> _userRepository;

        public UserCreationRule(ICrudRepository<Entities.User> userRepository)
        {
            _userRepository = userRepository;
        }

        public override RuleResultDto Execute(string email)
        {
            if (!IsParameterValid(email, out string validationMessage))
            {
                _result.Errors.Add(validationMessage);
            }

            var filter = new Dictionary<string, object>()
            {
                {"Email", email }
            };

            var user = _userRepository.GetByFilter(filter);

            if (user.Any())
            {
                _result.Errors.Add("E-mail already registered!");
            }

            return Resolve();
        }

        internal override bool IsParameterValid(string parameter, out string message)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                message = $"E-mail null or empty.";

                return false;
            }

            message = "E-mail valid";

            return true;
        }
    }
}
