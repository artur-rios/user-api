using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Utils;

namespace TechCraftsmen.User.Core.Entities
{
    public class User : FilterableEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public byte[] Password { get; set; } = [];

        public byte[] Salt { get; set; } = [];

        public int RoleId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool Active { get; set; } = true;

        private readonly Func<string, object> CreatedAtFilterParser = (value) => { var result = value.ToDateTime(); return result is not null ? result.Value : throw new NotAllowedException("Could not parse to DateTime"); };
        private readonly Func<string, object> RoleIdFilterParser = (value) => { var result = value.ToInt(); return result is not null ? result.Value : throw new NotAllowedException("Could not parse to int"); };

        public override Tuple<string, object> ParseAndValidateFilter(string key, object value)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, object> ParseAndValidateFilters(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override void SetFilters()
        {
            Filters = [
                new EntityFilter() { Name = "Name", Type = this.Name.GetType(), Validator = value => value.IsFilledString() },
                new EntityFilter() { Name = "Email", Type = this.Email.GetType(), Validator = value => value.IsFilledString() },
                new EntityFilter() { Name = "RoleId", Parser = RoleIdFilterParser, Type = this.RoleId.GetType(), Validator = value => value.IsIntBiggerThanZero() },
                new EntityFilter() { Name = "CreatedAt", Parser = CreatedAtFilterParser, Type = this.CreatedAt.GetType(), Validator = value => value.IsPastDateTime() },
                new EntityFilter() { Name = "Active", Type = this.Active.GetType(), Validator = value => value is bool },
            ];
        }
    }
}
