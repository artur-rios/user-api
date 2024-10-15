﻿using TechCraftsmen.User.Core.Filters;
using TechCraftsmen.User.Core.ValueObjects;
using TechCraftsmen.User.Services.Dto;

namespace TechCraftsmen.User.Services.Interfaces
{
    public interface IUserService
    {
        ServiceOutput<int> CreateUser(UserDto userDto);
        ServiceOutput<IList<UserDto>> GetUsersByFilter(UserFilter filter);
        ServiceOutput<HashOutput?> GetPasswordByUserId(int id);
        ServiceOutput<UserDto?> UpdateUser(UserDto userDto);
        ServiceOutput<int> ActivateUser(int id);
        ServiceOutput<int> DeactivateUser(int id);
        ServiceOutput<int>  DeleteUser(int id);
    }
}
