namespace TechCraftsmen.User.Tests.Utils.Mock
{
    public class UserGenerator
    {
        private readonly NameGenerator _nameGenerator = new();

        public Core.Entities.User Generate()
        {
            var id = CustomRandomNumberGenerator.RandomStrongInt();

            return new Core.Entities.User()
            {
                Id = id,
                Name = _nameGenerator.WithId(id).Generate()
            };
        }
    }
}
