using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Interfaces.Rules;
using TechCraftsmen.User.Core.Rules.Parameters;

namespace TechCraftsmen.User.Core.Rules
{
    public class UserCreationRule : IRule<string>
    {
        private readonly ICrudRepository<Entities.User> _userRepository;

        public UserCreationRule(ICrudRepository<Entities.User> userRepository)
        {
            _userRepository = userRepository;
        }

        public RuleResultDto Execute(RuleParameter<string> parameter)
        {
            throw new NotImplementedException();
        }
    }
}
