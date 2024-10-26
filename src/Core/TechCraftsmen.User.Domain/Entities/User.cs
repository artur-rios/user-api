using TechCraftsmen.User.Domain.Enums;

namespace TechCraftsmen.User.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public byte[] Password { get; set; } = [];

        public byte[] Salt { get; set; } = [];

        public int RoleId { get; init; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool Active { get; set; } = true;

        public bool CanActivate(out string[] errors)
        {
            if (!Active)
            {
                errors = [];
                
                return true;
            }

            errors = ["User already active"];

            return false;

        }
        
        public bool CanDeactivate(out string[] errors)
        {
            if (Active)
            {
                errors = [];
                
                return true;
            }

            errors = ["User already inactive"];

            return false;
        }
        
        public bool CanDelete(out string[] errors)
        {
            if (!Active)
            {
                errors = [];
                
                return true;
            }

            errors = ["Can't delete active user"];

            return false;
        }

        public bool CanRegister(int authenticatedRoleId, out string[] errors)
        {
            if (authenticatedRoleId == (int)Roles.Admin || RoleId == (int)Roles.Regular)
            {
                errors = [];

                return true;
            }

            Roles role = (Roles)RoleId;

            errors = [$"Only admins can register a user with {role.ToString()} role"];

            return false;
        }

        public bool CanUpdate(out string[] errors)
        {
            if (Active)
            {
                errors = [];

                return true;
            }

            errors = ["Can't update inactive user"];
            
            return false;
        }
    }
}
