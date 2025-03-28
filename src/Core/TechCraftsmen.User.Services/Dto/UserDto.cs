﻿using TechCraftsmen.User.Domain.Enums;

namespace TechCraftsmen.User.Services.Dto
{
    public class UserDto
    {
        public int Id { get; init; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int RoleId { get; init; } = (int)Roles.Regular;

        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        public bool Active { get; init; } = true;
    }
}
