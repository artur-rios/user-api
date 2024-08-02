using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Filters;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Interfaces.Services;
using TechCraftsmen.User.Core.Utils;

namespace TechCraftsmen.User.Services
{
    public class UserService(
        ICrudRepository<Core.Entities.User> userRepository,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IValidator<UserDto> userValidator,
        UserFilterValidator userFilter
    ) : IUserService
    {
        public int CreateUser(UserDto userDto)
        {
            userValidator.ValidateAndThrow(userDto);

            var filter = new Dictionary<string, object>() { { "Email", userDto.Email } };

            var userSearch = userRepository.GetByFilter(filter);

            if (userSearch.Any())
            {
                throw new NotAllowedException("E-mail already registered");
            }

            var user = mapper.Map<Core.Entities.User>(userDto);

            httpContextAccessor.HttpContext!.Items.TryGetValue("User", out var userData);

            var authenticatedUser = userData as UserDto;

            var canRegister = user.CanRegister(authenticatedUser!.RoleId);

            if (!canRegister.Success)
            {
                var exceptionMessage = string.Join(" | ", canRegister.Errors);

                throw new NotAllowedException(exceptionMessage);
            }

            var hashResult = HashUtils.HashText(userDto.Password);

            user.Password = hashResult.Hash;
            user.Salt = hashResult.Salt;

            return userRepository.Create(user);
        }

        public UserDto GetUserById(int id)
        {
            var user = userRepository.GetById(id);

            return user is null
                ? throw new NotFoundException("User not found!")
                : mapper.Map<UserDto>(user);
        }

        public IList<UserDto> GetUsersByFilter(IQueryCollection query)
        {
            Dictionary<string, object> validFilters = userFilter.ParseAndValidateFilters(query);

            if (validFilters.Count == 0)
            {
                throw new NotAllowedException("No valid filters were passed!");
            }

            var users = userRepository.GetByFilter(validFilters).ToList();

            if (users is null || users.Count == 0)
            {
                throw new NotFoundException("No users found with the given filter");
            }

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                userDtos.Add(mapper.Map<UserDto>(user));
            }

            return userDtos;
        }

        public HashDto GetPasswordByUserId(int id)
        {
            var user = userRepository.GetById(id);

            return user is null
                ? throw new NotFoundException("User not found!")
                : new HashDto() { Hash = user.Password, Salt = user.Salt };
        }

        public void UpdateUser(UserDto userDto)
        {
            var currentUser = userRepository.GetById(userDto.Id) ?? throw new NotFoundException("User not found!");

            var canUpdate = currentUser.CanUpdate();

            if (!canUpdate.Success)
            {
                var exceptionMessage = string.Join("|", canUpdate.Errors);

                throw new NotAllowedException(exceptionMessage);
            }

            var user = mapper.Map<Core.Entities.User>(userDto);

            MergeUser(user, currentUser);

            userRepository.Update(user);
        }

        public void ActivateUser(int id)
        {
            var user = userRepository.GetById(id) ?? throw new NotFoundException("User not found!");

            var canActivate = user.CanActivate();

            if (!canActivate.Success)
            {
                var exceptionMessage = string.Join("|", canActivate.Errors);

                throw new EntityNotChangedException(exceptionMessage);
            }

            user.Active = true;

            userRepository.Update(user);
        }

        public void DeactivateUser(int id)
        {
            var user = userRepository.GetById(id) ?? throw new NotFoundException("User not found!");

            var canDeactivate = user.CanDeactivate();

            if (!canDeactivate.Success)
            {
                var exceptionMessage = string.Join("|", canDeactivate.Errors);

                throw new EntityNotChangedException(exceptionMessage);
            }

            user.Active = false;

            userRepository.Update(user);
        }

        public void DeleteUser(int id)
        {
            var user = userRepository.GetById(id) ?? throw new NotFoundException("User not found!");

            var canDelete = user.CanDelete();

            if (!canDelete.Success)
            {
                var exceptionMessage = string.Join("|", canDelete.Errors);

                throw new NotAllowedException(exceptionMessage);
            }

            userRepository.Delete(user);
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