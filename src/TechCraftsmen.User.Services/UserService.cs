using AutoMapper;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Utils;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Interfaces.Services;

namespace TechCraftsmen.User.Core.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ICrudRepository<Entities.User> _userRepository;

        public UserService(IMapper mapper, ICrudRepository<Entities.User> userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public int CreateUser(UserDto userDto)
        {
            var user = _mapper.Map<Entities.User>(userDto);

            var hashResult = HashUtils.HashText(userDto.Password);

            user.Password = hashResult.Hash;
            user.Salt = hashResult.Salt;
            user.RoleId = (int)Roles.ADMIN;

            return _userRepository.Create(user);
        }

        public UserDto GetUserById(int id)
        {
            var user = _userRepository.GetById(id);

            return user is null
                ? throw new NotFoundException("User not found!")
                : _mapper.Map<UserDto>(user);
        }

        public void UpdateUser(UserDto userDto)
        {
            var currentUser = _userRepository.GetById(userDto.Id);

            if (!currentUser!.Active)
            {
                throw new NotAllowedException("Can't update inactive user!");
            }

            var user = _mapper.Map<Entities.User>(userDto);

            MergeUser(user, currentUser);

            _userRepository.Update(user);
        }

        public void ActivateUser(int id)
        {
            var user = _userRepository.GetById(id) ?? throw new NotFoundException("User not found!");

            if (user.Active)
            {
                throw new EntityNotChangedException("User already active!");
            }

            user.Active = true;

            _userRepository.Update(user);
        }

        public void DeactivateUser(int id)
        {
            var user = _userRepository.GetById(id) ?? throw new NotFoundException("User not found!");

            if (!user.Active)
            {
                throw new EntityNotChangedException("User already inactive!");
            }

            user.Active = false;

            _userRepository.Update(user);
        }

        public void DeleteUser(int id)
        {
            var user = _userRepository.GetById(id) ?? throw new NotFoundException("User not found!");

            if (user.Active)
            {
                throw new NotAllowedException("Can't delete active user!");
            }

            _userRepository.Delete(user);
        }

        private static void MergeUser(Entities.User source, Entities.User target, bool mergeStatus = true)
        {
            target.Password = source.Password;
            target.CreatedAt = source.CreatedAt;

            if (mergeStatus)
            {
                target.Active = source.Active;
            }
        }
    }
}
