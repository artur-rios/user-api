﻿using Xunit.Abstractions;
using Xunit.Sdk;

namespace TechCraftsmen.User.Tests.Utils.Traits
{
    [TraitDiscoverer(UnitDiscoverer.DISCOVERER_TYPE_NAME, TraitConstants.ASSEMBLY_NAME)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class UnitAttribute(string name) : Attribute, ITraitAttribute
    {
        public string Name { get; private set; } = name;
    }

    public class UnitDiscoverer : ITraitDiscoverer
    {
        internal const string DISCOVERER_TYPE_NAME = $"{TraitConstants.ASSEMBLY_NAME}.{nameof(UnitDiscoverer)}";

        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            string? name = traitAttribute.GetNamedArgument<string>("Name");

            if (!string.IsNullOrWhiteSpace(name))
            {
                yield return new KeyValuePair<string, string>("Unit", name);
            }

            yield return new KeyValuePair<string, string>("Trait", "Unit");
        }
    }
}
