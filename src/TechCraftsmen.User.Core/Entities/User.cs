using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;

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

        public SimpleResultDto CanActivate()
        {
            SimpleResultDto result = new();
            
            if (Active)
            {
                result.Errors.Add($"User already active");
            }
            
            result.SetSuccessByErrors();

            return result;
        }
        
        public SimpleResultDto CanDeactivate()
        {
            SimpleResultDto result = new();
            
            if (!Active)
            {
                result.Errors.Add($"User already inactive");
            }
            
            result.SetSuccessByErrors();

            return result;
        }
        
        public SimpleResultDto CanDelete()
        {
            SimpleResultDto result = new();
            
            if (Active)
            {
                result.Errors.Add("Can't delete active user");
            }
            
            result.SetSuccessByErrors();

            return result;
        }

        public SimpleResultDto CanRegister(int authenticatedRoleId)
        {
            SimpleResultDto result = new();
            
            if (RoleId != (int)Roles.Regular)
            {
                if (authenticatedRoleId != (int)Roles.Admin)
                {
                    Roles role = (Roles)RoleId;
                    
                    result.Errors.Add($"Only admins can register a user with {role.ToString()} role");
                }
            }
            
            result.SetSuccessByErrors();

            return result;
        }

        public SimpleResultDto CanUpdate()
        {
            SimpleResultDto result = new();
            
            if (!Active)
            {
                result.Errors.Add("Can't update inactive user");
            }
            
            result.SetSuccessByErrors();

            return result;
        }
    }
}
