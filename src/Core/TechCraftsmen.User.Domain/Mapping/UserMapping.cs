using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Core.Mapping
{
    public static class UserMapping
    {
        public static UserDto ToDto(this Entities.User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                RoleId = user.RoleId,
                CreatedAt = user.CreatedAt,
                Active = user.Active
            };
        }

        public static Entities.User ToEntity(this UserDto dto)
        {
            return new Entities.User
            {
                Id = dto.Id,
                Email = dto.Email,
                Name = dto.Name,
                RoleId = dto.RoleId,
                CreatedAt = dto.CreatedAt,
                Active = dto.Active
            };
        }
    }
}
