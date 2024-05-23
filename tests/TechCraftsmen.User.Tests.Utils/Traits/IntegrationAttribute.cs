﻿using Xunit.Abstractions;
using Xunit.Sdk;

namespace TechCraftsmen.User.Tests.Utils.Traits
{
    [TraitDiscoverer(UnitDiscoverer.DiscovererTypeName, TraitConstants.AssemblyName)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class IntegrationAttribute(string name) : Attribute, ITraitAttribute
    {
        public string Name { get; private set; } = name;
    }

    public class IntegrationDiscoverer : ITraitDiscoverer
    {
        internal const string DiscovererTypeName = $"{TraitConstants.AssemblyName}.{nameof(UnitDiscoverer)}";

        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            var name = traitAttribute.GetNamedArgument<string>("Name");

            if (!string.IsNullOrWhiteSpace(name))
            {
                yield return new KeyValuePair<string, string>("Integration", name);
            }


            yield return new KeyValuePair<string, string>("Trait", "Integration");
        }
    }
}
