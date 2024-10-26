using TechCraftsmen.User.Services.Dto;
using TechCraftsmen.User.Services.Filters;
using TechCraftsmen.User.Services.Output;

namespace TechCraftsmen.User.Services.Interfaces
{
    public interface IUserService
    {
        ServiceOutput<int> CreateUser(UserDto userDto);
        ServiceOutput<IList<UserDto>> GetUsersByFilter(UserFilter filter);
        ServiceOutput<UserDto?> UpdateUser(UserDto userDto);
        ServiceOutput<int> ActivateUser(int id);
        ServiceOutput<int> DeactivateUser(int id);
        ServiceOutput<int>  DeleteUser(int id);
    }
}
