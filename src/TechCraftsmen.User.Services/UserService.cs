using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Filters;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Interfaces.Services;
using TechCraftsmen.User.Core.Rules.User;
using TechCraftsmen.User.Core.Utils;

namespace TechCraftsmen.User.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ICrudRepository<Core.Entities.User> _userRepository;
        private readonly IValidator<UserDto> _userValidator;
        private readonly UserFilterValidator _filterValidator;
        private readonly UserCreationRule _creationRule;
        private readonly UserUpdateRule _updateRule;
        private readonly UserStatusUpdateRule _statusUpdateRule;
        private readonly UserDeletionRule _deletionRule;

        public UserService(IMapper mapper, ICrudRepository<Core.Entities.User> userRepository, IValidator<UserDto> userValidator, UserFilterValidator userFilter, UserCreationRule creationRule, UserUpdateRule updateRule, UserStatusUpdateRule statusUpdateRule, UserDeletionRule deletionRule)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _userValidator = userValidator;
            _filterValidator = userFilter;
            _creationRule = creationRule;
            _updateRule = updateRule;
            _statusUpdateRule = statusUpdateRule;
            _deletionRule = deletionRule;
        }

        public int CreateUser(UserDto userDto)
        {
            _userValidator.ValidateAndThrow(userDto);

            var ruleResult = _creationRule.Execute(userDto.Email);

            if (!ruleResult.Success)
            {
                var exceptionMessage = string.Join("|", ruleResult.Errors);

                throw new NotAllowedException(exceptionMessage);
            }

            var user = _mapper.Map<Core.Entities.User>(userDto);

            var hashResult = HashUtils.HashText(userDto.Password);

            user.Password = hashResult.Hash;
            user.Salt = hashResult.Salt;

            return _userRepository.Create(user);
        }

        public UserDto GetUserById(int id)
        {
            var user = _userRepository.GetById(id);

            return user is null
                ? throw new NotFoundException("User not found!")
                : _mapper.Map<UserDto>(user);
        }

        public IList<UserDto> GetUsersByFilter(IQueryCollection query)
        {
            Dictionary<string, object> validFilters = _filterValidator.ParseAndValidateFilters(query);

            if (validFilters.Count == 0)
            {
                throw new NotAllowedException("No valid filters were passed!");
            }

            var users = _userRepository.GetByFilter(validFilters).ToList();

            if (users is null || users.Count == 0)
            {
                throw new NotFoundException("No users found with the given filter");
            }

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                userDtos.Add(_mapper.Map<UserDto>(user));
            }

            return userDtos;
        }

        public HashDto GetPasswordByUserId(int id)
        {
            var user = _userRepository.GetById(id);

            return user is null
                ? throw new NotFoundException("User not found!")
                : new HashDto() { Hash = user.Password, Salt = user.Salt };
        }

        public void UpdateUser(UserDto userDto)
        {
            var currentUser = _userRepository.GetById(userDto.Id) ?? throw new NotFoundException("User not found!");

            var ruleResult = _updateRule.Execute(currentUser!.Active);

            if (!ruleResult.Success)
            {
                var exceptionMessage = string.Join("|", ruleResult.Errors);

                throw new NotAllowedException(exceptionMessage);
            }

            var user = _mapper.Map<Core.Entities.User>(userDto);

            MergeUser(user, currentUser);

            _userRepository.Update(user);
        }

        public void ActivateUser(int id)
        {
            var user = _userRepository.GetById(id) ?? throw new NotFoundException("User not found!");

            var ruleResult = _statusUpdateRule.Execute(Tuple.Create(user.Active, true));

            if (!ruleResult.Success)
            {
                var exceptionMessage = string.Join("|", ruleResult.Errors);

                throw new EntityNotChangedException(exceptionMessage);
            }

            user.Active = true;

            _userRepository.Update(user);
        }

        public void DeactivateUser(int id)
        {
            var user = _userRepository.GetById(id) ?? throw new NotFoundException("User not found!");

            var ruleResult = _statusUpdateRule.Execute(Tuple.Create(user.Active, false));

            if (!ruleResult.Success)
            {
                var exceptionMessage = string.Join("|", ruleResult.Errors);

                throw new EntityNotChangedException(exceptionMessage);
            }

            user.Active = false;

            _userRepository.Update(user);
        }

        public void DeleteUser(int id)
        {
            var user = _userRepository.GetById(id) ?? throw new NotFoundException("User not found!");

            var ruleResult = _deletionRule.Execute(user.Active);

            if (!ruleResult.Success)
            {
                var exceptionMessage = string.Join("|", ruleResult.Errors);

                throw new NotAllowedException(exceptionMessage);
            }

            _userRepository.Delete(user);
        }

        private static void MergeUser(Core.Entities.User source, Core.Entities.User target, bool mergeStatus = true)
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
