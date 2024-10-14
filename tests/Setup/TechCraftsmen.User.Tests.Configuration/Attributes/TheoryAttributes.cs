using Xunit.Sdk;

namespace TechCraftsmen.User.Tests.Utils.Attributes
{
    [XunitTestCaseDiscoverer("Xunit.Sdk.TheoryDiscoverer", "xunit.execution.{Platform}")]
    [AttributeUsage(AttributeTargets.Method)]
    public class FunctionalTheoryAttribute(string name, string[]? environments = null)
        : BaseFactAttribute(name, environments);
    
    [XunitTestCaseDiscoverer("Xunit.Sdk.TheoryDiscoverer", "xunit.execution.{Platform}")]
    [AttributeUsage(AttributeTargets.Method)]
    public class UnitTheoryAttribute(string name, string[]? environments = null)
        : BaseFactAttribute(name, environments);
}
