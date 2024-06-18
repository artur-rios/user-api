namespace TechCraftsmen.User.Core.Filters
{
    public class EntityFilter
    {
        public string Name { get; set; }
        public Func<string, object> Parser { get; set; }
        public Func<object, bool> Validator { get; set; }
        public Type @Type { get; set; }

        private readonly Func<string, object> _defaultParser = (value) => value;

        public EntityFilter(string name, Type type, Func<object, bool> validator, Func<string, object>? parser = null)
        {
            Name = name;
            Parser = parser is null ? _defaultParser : parser;
            Type = type;
            Validator = validator;
        }

        public object? ParseAndValidateValue(string value)
        {
            var parsedValue = Parser(value);

            return Validator(parsedValue) ? parsedValue : null;
        }
    }
}
