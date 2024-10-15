namespace TechCraftsmen.User.Tests.Configuration.Attributes
{
    public class FunctionalFactAttribute(string name, string[]? environments = null)
        : BaseFactAttribute(name, environments);
    
    public class UnitFactAttribute(string name, string[]? environments = null)
        : BaseFactAttribute(name, environments);
}
