using FluentValidation;
using Microsoft.AspNetCore.Http;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Extensions;
using TechCraftsmen.User.Core.Filters;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Interfaces.Services;
using TechCraftsmen.User.Core.Mapping;
using TechCraftsmen.User.Core.Utils;

namespace TechCraftsmen.User.Services
{
    public class UserService(
        ICrudRepository<Core.Entities.User> userRepository,
        IHttpContextAccessor httpContextAccessor,
        IValidator<UserDto> userValidator
    ) : IUserService
    {
        public int CreateUser(UserDto userDto)
        {
            userValidator.ValidateAndThrow(userDto);

            Dictionary<string, object> filter = new() { { "Email", userDto.Email } };

            IQueryable<Core.Entities.User> userSearch = userRepository.GetByFilter(filter);

            if (userSearch.Any())
            {
                throw new NotAllowedException("E-mail already registered");
            }

            Core.Entities.User user = userDto.ToEntity();

            httpContextAccessor.HttpContext!.Items.TryGetValue("User", out object? userData);

            UserDto? authenticatedUser = userData as UserDto;

            SimpleResultDto canRegister = user.CanRegister(authenticatedUser!.RoleId);

            if (!canRegister.Success)
            {
                string exceptionMessage = string.Join(" | ", canRegister.Errors);

                throw new NotAllowedException(exceptionMessage);
            }

            HashDto hashResult = HashUtils.HashText(userDto.Password);

            user.Password = hashResult.Hash;
            user.Salt = hashResult.Salt;

            return userRepository.Create(user);
        }

        public IList<UserDto> GetUsersByFilter(UserFilter filter)
        {
            List<Core.Entities.User> users = userRepository.GetByFilter(filter.NonNullPropertiesToDictionary()).ToList();

            if (users is null || users.Count == 0)
            {
                throw new NotFoundException("No users found with the given filter");
            }

            return users.Select(user => user.ToDto()).ToList();
        }

        public HashDto GetPasswordByUserId(int id)
        {
            Core.Entities.User? user = userRepository.GetByFilter(id.ToDictionary("Id")).FirstOrDefault();

            return user is null
                ? throw new NotFoundException("User not found!")
                : new HashDto() { Hash = user.Password, Salt = user.Salt };
        }

        public void UpdateUser(UserDto userDto)
        {
            Core.Entities.User currentUser = userRepository.GetByFilter(userDto.Id.ToDictionary("Id")).FirstOrDefault() ?? throw new NotFoundException("User not found!");

            SimpleResultDto canUpdate = currentUser.CanUpdate();

            if (!canUpdate.Success)
            {
                string exceptionMessage = string.Join(" | ", canUpdate.Errors);

                throw new NotAllowedException(exceptionMessage);
            }

            Core.Entities.User user = userDto.ToEntity();

            MergeUser(user, currentUser);

            userRepository.Update(user);
        }

        public void ActivateUser(int id)
        {
            Core.Entities.User user = userRepository.GetByFilter(id.ToDictionary("Id")).FirstOrDefault() ?? throw new NotFoundException("User not found!");

            SimpleResultDto canActivate = user.CanActivate();

            if (!canActivate.Success)
            {
                string exceptionMessage = string.Join(" | ", canActivate.Errors);

                throw new EntityNotChangedException(exceptionMessage);
            }

            user.Active = true;

            userRepository.Update(user);
        }

        public void DeactivateUser(int id)
        {
            Core.Entities.User user = userRepository.GetByFilter(id.ToDictionary("Id")).FirstOrDefault() ?? throw new NotFoundException("User not found!");

            SimpleResultDto canDeactivate = user.CanDeactivate();

            if (!canDeactivate.Success)
            {
                string exceptionMessage = string.Join(" | ", canDeactivate.Errors);

                throw new EntityNotChangedException(exceptionMessage);
            }

            user.Active = false;

            userRepository.Update(user);
        }

        public void DeleteUser(int id)
        {
            Core.Entities.User user = userRepository.GetByFilter(id.ToDictionary("Id")).FirstOrDefault() ?? throw new NotFoundException("User not found!");

            SimpleResultDto canDelete = user.CanDelete();

            if (!canDelete.Success)
            {
                string exceptionMessage = string.Join(" | ", canDelete.Errors);

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