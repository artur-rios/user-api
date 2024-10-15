using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.ValueObjects;

namespace TechCraftsmen.User.Core.Entities
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

        public DomainOutput CanActivate()
        {
            DomainOutput result = new();
            
            if (Active)
            {
                result.Errors.Add("User already active");
            }

            return result;
        }
        
        public DomainOutput CanDeactivate()
        {
            DomainOutput result = new();
            
            if (!Active)
            {
                result.Errors.Add("User already inactive");
            }

            return result;
        }
        
        public DomainOutput CanDelete()
        {
            DomainOutput result = new();
            
            if (Active)
            {
                result.Errors.Add("Can't delete active user");
            }

            return result;
        }

        public DomainOutput CanRegister(int authenticatedRoleId)
        {
            DomainOutput result = new();
            
            if (RoleId != (int)Roles.Regular)
            {
                if (authenticatedRoleId != (int)Roles.Admin)
                {
                    Roles role = (Roles)RoleId;
                    
                    result.Errors.Add($"Only admins can register a user with {role.ToString()} role");
                }
            }

            return result;
        }

        public DomainOutput CanUpdate()
        {
            DomainOutput result = new();
            
            if (!Active)
            {
                result.Errors.Add("Can't update inactive user");
            }

            return result;
        }
    }
}
