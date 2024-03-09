using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Core.Interfaces.Rules
{
    public interface IRule<T>
    {
        RuleResultDto Execute(T parameter);
    }
}
