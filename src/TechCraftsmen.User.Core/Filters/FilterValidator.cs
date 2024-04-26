using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using TechCraftsmen.User.Core.Extensions;

namespace TechCraftsmen.User.Core.Filters;

public abstract class FilterValidator
{
    public List<EntityFilter> Filters = [];

    public FilterValidator()
    {
        SetFilters();
    }

    public abstract void SetFilters();

    public KeyValuePair<string, string>? ParseAndValidateFilter(KeyValuePair<string, StringValues> filter)
    {
        var searchedFilter = Filters.Find(f => f.Name == filter.Key);

        if (searchedFilter is not null)
        {
            var value = searchedFilter.ParseAndValidateValue(filter.Value!);

            if (value is not null)
            {
                return new KeyValuePair<string, string>(filter.Key, value);
            }
        }

        return null;
    }

    public Dictionary<string, object> ParseAndValidateFilters(IQueryCollection query)
    {
        var parsedFilters = new Dictionary<string, object>();

        if (query is not null || query?.Count > 0)
        {
            foreach (var item in query)
            {
                var parsedFilter = ParseAndValidateFilter(item);

                if (parsedFilter is not null)
                {
                    parsedFilter.Value.Deconstruct<string, string>(out var key, out var value);

                    parsedFilters.Add(key, value);
                }
            }
        }

        return parsedFilters;
    }
}
