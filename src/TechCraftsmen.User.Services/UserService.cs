using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Interfaces.Services;

namespace TechCraftsmen.User.Core.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly ICrudRepository<Entities.User> _userRepository;

        public UserService(ICrudRepository<Entities.User> userRepository)
        {
            _userRepository = userRepository;
        }

        public int CreateUser(Entities.User user)
        {
            return _userRepository.Create(user);
        }

        public Entities.User GetUserById(int id)
        {
            var user = _userRepository.GetById(id);

            return user is null
                ? throw new NotFoundException("User not found!")
                : user;
        }

        public void UpdateUser(Entities.User user)
        {
            _userRepository.Update(user);
        }

        public void ActivateUser(int id)
        {
            var user = GetUserById(id);

            if (user.Active)
            {
                throw new EntityNotChangedException("User already active");
            }

            user.Active = true;

            UpdateUser(user);
        }

        public void DeactivateUser(int id)
        {
            var user = GetUserById(id);

            if (!user.Active)
            {
                throw new EntityNotChangedException("User already inactive");
            }

            user.Active = false;

            UpdateUser(user);
        }

        public void DeleteUser(int id)
        {
            var user = GetUserById(id);

            if (user.Active)
            {
                throw new NotAllowedException("Can't delete active user");
            }

            _userRepository.Delete(user);
        }
    }
}
