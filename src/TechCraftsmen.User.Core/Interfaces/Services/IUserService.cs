namespace TechCraftsmen.User.Core.Interfaces.Services
{
    public interface IUserService
    {
        int CreateUser(Entities.User user);
        Entities.User GetUserById(int id);
        void UpdateUser(Entities.User user);
        public void ActivateUser(int id);
        public void DeactivateUser(int id);
        void DeleteUser(int id);
    }
}
