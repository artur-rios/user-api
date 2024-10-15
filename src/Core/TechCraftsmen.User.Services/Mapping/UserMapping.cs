using TechCraftsmen.User.Services.Dto;

namespace TechCraftsmen.User.Services.Mapping
{
    public static class UserMapping
    {
        public static UserDto ToDto(this Core.Entities.User user)
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

        public static Core.Entities.User ToEntity(this UserDto dto)
        {
            return new Core.Entities.User
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
