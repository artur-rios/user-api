using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Interfaces.Rules;
using TechCraftsmen.User.Core.Rules.Parameters;

namespace TechCraftsmen.User.Core.Rules
{
    public class UserUpdateRule : IRule<bool>
    {
        public RuleResultDto Execute(RuleParameter<bool> parameter)
        {
            var result = new RuleResultDto();

            if (!parameter.Value)
            {
                result.Errors.Add("Can't update inactive user!");
            }

            result.Success = !result.Errors.Any();

            return result;
        }
    }
}
