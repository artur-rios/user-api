// ReSharper disable UnusedMember.Global
// Reason: the empty constructor is necessary and the "unused" properties too
namespace TechCraftsmen.User.Services.Filters
{
    public class UserFilter
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? Active { get; set; }
        
        public UserFilter()
        {
        }

        public UserFilter(int id)
        {
            Id = id;
        }

        public UserFilter(string email)
        {
            Email = email;
        }
    }
}
