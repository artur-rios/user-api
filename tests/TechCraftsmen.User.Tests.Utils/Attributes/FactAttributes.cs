namespace TechCraftsmen.User.Tests.Utils.Attributes
{
    public class FunctionalFactAttribute(string name, string[]? environments = null)
        : BaseFactAttribute(name, environments);
    
    public class UnitFactAttribute(string name, string[]? environments = null)
        : BaseFactAttribute(name, environments);
}
