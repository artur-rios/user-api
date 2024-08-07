namespace TechCraftsmen.User.Tests.Utils.Generators
{
    public class NameGenerator
    {
        private const string DefaultUserName = "Jhon Doe";

        private int? _id = null;

        public NameGenerator WithId(int id)
        {
            _id = id;

            return this;
        }

        public NameGenerator WithRandomId()
        {
            _id = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, 999);

            return this;
        }

        public string Generate()
        {
            string name = DefaultUserName;

            if (_id is not null)
            {
                name += $" {_id}";
            }

            return name;
        }
    }
}
