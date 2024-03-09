using TechCraftsmen.User.Common.Dto;

namespace TechCraftsmen.User.Core.Interfaces.Rules
{
    public interface IRule
    {
        RuleResultDto Execute(params object[] ruleParameters);
    }
}
