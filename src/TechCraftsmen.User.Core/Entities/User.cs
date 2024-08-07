using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;

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

        public SimpleResultDto CanActivate()
        {
            SimpleResultDto result = new SimpleResultDto();
            
            if (Active)
            {
                result.Errors.Add($"User already active");
            }
            
            result.SetSuccessByErrors();

            return result;
        }
        
        public SimpleResultDto CanDeactivate()
        {
            SimpleResultDto result = new SimpleResultDto();
            
            if (!Active)
            {
                result.Errors.Add($"User already inactive");
            }
            
            result.SetSuccessByErrors();

            return result;
        }
        
        public SimpleResultDto CanDelete()
        {
            SimpleResultDto result = new SimpleResultDto();
            
            if (Active)
            {
                result.Errors.Add("Can't delete active user");
            }
            
            result.SetSuccessByErrors();

            return result;
        }

        public SimpleResultDto CanRegister(int authenticatedRoleId)
        {
            SimpleResultDto result = new SimpleResultDto();
            
            if (RoleId != (int)Roles.Regular)
            {
                if (authenticatedRoleId != (int)Roles.Admin)
                {
                    result.Errors.Add("Only admins can register this kind of user");
                }
            }
            
            result.SetSuccessByErrors();

            return result;
        }

        public SimpleResultDto CanUpdate()
        {
            SimpleResultDto result = new SimpleResultDto();
            
            if (!Active)
            {
                result.Errors.Add("Can't update inactive user");
            }
            
            result.SetSuccessByErrors();

            return result;
        }
    }
}
