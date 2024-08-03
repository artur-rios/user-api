using Microsoft.AspNetCore.Http;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Filters;

namespace TechCraftsmen.User.Core.Interfaces.Services
{
    public interface IUserService
    {
        int CreateUser(UserDto userDto);
        IList<UserDto> GetUsersByFilter(UserFilter filter);
        HashDto GetPasswordByUserId(int id);
        void UpdateUser(UserDto userDto);
        public void ActivateUser(int id);
        public void DeactivateUser(int id);
        void DeleteUser(int id);
    }
}
