using Microsoft.AspNetCore.Http;
using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Core.Interfaces.Services
{
    public interface IUserService
    {
        int CreateUser(UserDto userDto);
        UserDto GetUserById(int id);
        IEnumerable<UserDto> GetUsersByFilter(IQueryCollection query);
        void UpdateUser(UserDto userDto);
        public void ActivateUser(int id);
        public void DeactivateUser(int id);
        void DeleteUser(int id);
    }
}
