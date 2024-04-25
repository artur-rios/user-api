namespace TechCraftsmen.User.Core;

public class EntityFilter
{
    public string Name { get; set; } = string.Empty;
    public Func<string, object> Parser { get; set; } = (value) => value;
    public Func<object, bool> Validator { get; set; } = (value) => true;
    public Type @Type { get; set; } = typeof(string);
}
