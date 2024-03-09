using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Rules.Parameters;

namespace TechCraftsmen.User.Core.Interfaces.Rules
{
    public interface IRule<T>
    {
        RuleResultDto Execute(RuleParameter<T> parameter);
    }
}
