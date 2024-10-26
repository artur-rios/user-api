namespace TechCraftsmen.User.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;
    }
}
