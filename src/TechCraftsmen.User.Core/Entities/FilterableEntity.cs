using TechCraftsmen.User.Core.Entities;

namespace TechCraftsmen.User.Core;

public abstract class FilterableEntity : BaseEntity
{
    public List<EntityFilter> Filters { get; set; } = [];

    public FilterableEntity()
    {
        SetFilters();
    }

    public abstract Tuple<string, object> ParseAndValidateFilter(string key, object value);
    public abstract Dictionary<string, object> ParseAndValidateFilters(Dictionary<string, object> filters);
    public abstract void SetFilters();
}
