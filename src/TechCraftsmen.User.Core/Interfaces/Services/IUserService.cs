using TechCraftsmen.User.Common.Dto;

namespace TechCraftsmen.User.Core.Interfaces.Services
{
    public interface IUserService
    {
        int CreateUser(UserDto userDto);
        UserDto GetUserById(int id);
        void UpdateUser(UserDto userDto);
        public void ActivateUser(int id);
        public void DeactivateUser(int id);
        void DeleteUser(int id);
    }
}
