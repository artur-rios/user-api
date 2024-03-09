namespace TechCraftsmen.User.Core.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public byte[] Password { get; set; } = [];

        public byte[] Salt { get; set; } = [];

        public int RoleId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool Active { get; set; } = true;

        public HashSet<string> Filters { get; private set; } = ["Name", "Email", "RoleId", "CreateAt", "Active"];
    }
}
