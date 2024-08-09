using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Filters;

namespace TechCraftsmen.User.Core.Interfaces.Services
{
    public interface IUserService
    {
        OperationResultDto<int> CreateUser(UserDto userDto);
        OperationResultDto<IList<UserDto>> GetUsersByFilter(UserFilter filter);
        OperationResultDto<HashDto?> GetPasswordByUserId(int id);
        OperationResultDto<UserDto?> UpdateUser(UserDto userDto);
        OperationResultDto<int> ActivateUser(int id);
        OperationResultDto<int> DeactivateUser(int id);
        OperationResultDto<int>  DeleteUser(int id);
    }
}
