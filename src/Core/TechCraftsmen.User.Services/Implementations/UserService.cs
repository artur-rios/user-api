using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using TechCraftsmen.User.Core.Extensions;
using TechCraftsmen.User.Core.Filters;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Utils;
using TechCraftsmen.User.Core.ValueObjects;
using TechCraftsmen.User.Services.Dto;
using TechCraftsmen.User.Services.Interfaces;
using TechCraftsmen.User.Services.Mapping;
using Results = TechCraftsmen.User.Core.Enums.Results;

namespace TechCraftsmen.User.Services.Implementations
{
    public class UserService(
        ICrudRepository<Core.Entities.User> userRepository,
        IHttpContextAccessor httpContextAccessor,
        IValidator<UserDto> userValidator
    ) : IUserService
    {
        public ServiceOutput<int> CreateUser(UserDto userDto)
        {
            ValidationResult? validationResult = userValidator.Validate(userDto);
            
            string[] validationErrors = validationResult.Errors.Select(vf => vf.ErrorMessage).ToArray();

            if (!validationResult.IsValid)
            {
                return new ServiceOutput<int>(default, validationErrors,
                    Results.ValidationError);
            }

            Dictionary<string, object> filter = new() { { "Email", userDto.Email } };

            IQueryable<Core.Entities.User> userSearch = userRepository.GetByFilter(filter, true);

            if (userSearch.Any())
            {
                return new ServiceOutput<int>(default, ["E-mail already registered"], Results.NotAllowed);
            }

            Core.Entities.User user = userDto.ToEntity();

            httpContextAccessor.HttpContext!.Items.TryGetValue("User", out object? userData);
            
            UserDto? authenticatedUser = userData as UserDto;
            
            DomainOutput canRegister = user.CanRegister(authenticatedUser!.RoleId);
            
            if (!canRegister.Success)
            {
                return new ServiceOutput<int>(default, canRegister.Errors.ToArray(), Results.NotAllowed);
            }

            HashOutput hashResult = HashUtils.HashText(userDto.Password);

            user.Password = hashResult.Hash;
            user.Salt = hashResult.Salt;

            int createdId = userRepository.Create(user);

            return new ServiceOutput<int>(createdId, ["User created with success"], Results.Created);
        }

        public ServiceOutput<IList<UserDto>> GetUsersByFilter(UserFilter filter)
        {
            List<Core.Entities.User> users = userRepository.GetByFilter(filter.NonNullPropertiesToDictionary(), false).ToList();

            if (users.Count == 0)
            {
                return new ServiceOutput<IList<UserDto>>([], ["No users found for the given filter"], Results.NotFound);
            }

            List<UserDto> usersFound = users.Select(user => user.ToDto()).ToList();

            return new ServiceOutput<IList<UserDto>>(usersFound, ["Search completed with success"]);
        }

        public ServiceOutput<HashOutput?> GetPasswordByUserId(int id)
        {
            Core.Entities.User? user = userRepository.GetByFilter(id.ToDictionary("Id"), true).FirstOrDefault();

            return user is null
                ? new ServiceOutput<HashOutput?>(default, ["User not found"], Results.NotFound)
                : new ServiceOutput<HashOutput?>(new HashOutput() { Hash = user.Password, Salt = user.Salt }, ["Password found"]);
        }

        public ServiceOutput<UserDto?> UpdateUser(UserDto userDto)
        {
            Core.Entities.User? currentUser = userRepository.GetByFilter(userDto.Id.ToDictionary("Id"), false).FirstOrDefault();
            
            if (currentUser == null)
            {
                return new ServiceOutput<UserDto?>(default, ["User not found"], Results.NotFound);
            }

            DomainOutput canUpdate = currentUser.CanUpdate();

            if (!canUpdate.Success)
            {
                return new ServiceOutput<UserDto?>(default, canUpdate.Errors.ToArray(), Results.NotAllowed);
            }

            Core.Entities.User user = userDto.ToEntity();

            MergeUser(currentUser, user);

            userRepository.Update(user);

            return new ServiceOutput<UserDto?>(user.ToDto(), ["User updated with success"]);
        }

        public ServiceOutput<int> ActivateUser(int id)
        {
            Core.Entities.User? user = userRepository.GetByFilter(id.ToDictionary("Id"), false).FirstOrDefault();
            
            if (user == null)
            {
                return new ServiceOutput<int>(default, ["User not found"], Results.NotFound);
            }

            DomainOutput canActivate = user.CanActivate();

            if (!canActivate.Success)
            {
                return new ServiceOutput<int>(id, canActivate.Errors.ToArray(), Results.EntityNotChanged);
            }

            user.Active = true;

            userRepository.Update(user);

            return new ServiceOutput<int>(id, ["User activated with success"]);
        }

        public ServiceOutput<int> DeactivateUser(int id)
        {
            Core.Entities.User? user = userRepository.GetByFilter(id.ToDictionary("Id"), false).FirstOrDefault();
            
            if (user == null)
            {
                return new ServiceOutput<int>(default, ["User not found"], Results.NotFound);
            }

            DomainOutput canDeactivate = user.CanDeactivate();

            if (!canDeactivate.Success)
            {
                return new ServiceOutput<int>(id, canDeactivate.Errors.ToArray(), Results.EntityNotChanged);
            }

            user.Active = false;

            userRepository.Update(user);
            
            return new ServiceOutput<int>(id, ["User deactivated with success"]);
        }

        public ServiceOutput<int> DeleteUser(int id)
        {
            Core.Entities.User? user = userRepository.GetByFilter(id.ToDictionary("Id"), true).FirstOrDefault();
            
            if (user == null)
            {
                return new ServiceOutput<int>(default, ["User not found"], Results.NotFound);
            }

            DomainOutput canDelete = user.CanDelete();

            if (!canDelete.Success)
            {
                return new ServiceOutput<int>(id, canDelete.Errors.ToArray(), Results.NotAllowed);
            }

            userRepository.Delete(user);
            
            return new ServiceOutput<int>(id, ["User deleted with success"]);
        }

        private static void MergeUser(Core.Entities.User source, Core.Entities.User target, bool mergeStatus = true)
        {
            target.Password = source.Password;
            target.Salt = source.Salt;
            target.CreatedAt = source.CreatedAt;

            if (mergeStatus)
            {
                target.Active = source.Active;
            }
        }
    }
}
