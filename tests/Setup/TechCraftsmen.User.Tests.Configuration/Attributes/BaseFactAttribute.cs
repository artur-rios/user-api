using Xunit;

namespace TechCraftsmen.User.Tests.Configuration.Attributes
{
    public class BaseFactAttribute : FactAttribute
    {
        public string Name { get; private set; }
        public string[]? Environments { get; private set; }
        
        protected BaseFactAttribute(string name, string[]? environments = null)
        {
            Name = name;
            Environments = environments;

            if (environments is null)
            {
                return;
            }

            string? currentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            string? hasEnvironment = environments.FirstOrDefault(environment => environment == currentEnvironment);

            if (string.IsNullOrWhiteSpace(hasEnvironment))
            {
                // ReSharper disable once VirtualMemberCallInConstructor
                Skip = $"Test can't run on {currentEnvironment}";
            }
        }
    }
}