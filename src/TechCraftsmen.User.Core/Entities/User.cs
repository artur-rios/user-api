namespace TechCraftsmen.User.Core.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Salt { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool Active { get; set; } = true;
    }
}
