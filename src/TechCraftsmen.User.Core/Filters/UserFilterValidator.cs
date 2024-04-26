using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Extensions;

namespace TechCraftsmen.User.Core.Filters;

public class UserFilterValidator : FilterValidator
{
    private readonly EntityFilter _nameFilter = new("Name", typeof(string), value => value.IsFilledString());
    private readonly EntityFilter _emailFilter = new("Email", typeof(string), value => value.IsFilledString());
    private readonly EntityFilter _roleIdFilter = new("RoleId", typeof(int), value => value.IsIntBiggerThanZero());
    private readonly EntityFilter _createdAtFilter = new("CreatedAt", typeof(DateTime), value => value.IsPastDateTime());
    private readonly EntityFilter _activeFilter = new("Active", typeof(bool), value => value is bool);

    private readonly Func<string, object> _createdAtFilterParser = (value) => { var result = value.ToDateTime(); return result is not null ? result.Value : throw new NotAllowedException("Could not parse to DateTime"); };
    private readonly Func<string, object> _roleIdFilterParser = (value) => { var result = value.ToInt(); return result is not null ? result.Value : throw new NotAllowedException("Could not parse to int"); };

    public override void SetFilters()
    {
        _roleIdFilter.Parser = _roleIdFilterParser;
        _createdAtFilter.Parser = _createdAtFilterParser;

        List<EntityFilter> filters = [_nameFilter, _emailFilter, _roleIdFilter, _createdAtFilter, _activeFilter];

        Filters = filters;
    }
}
