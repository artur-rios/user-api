namespace TechCraftsmen.User.Core.Rules.Parameters
{
    public abstract class RuleParameter<T>
    {
        public T Value { get; set; } = default!;
    }
}
