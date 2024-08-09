using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Extensions;
using TechCraftsmen.User.Core.Filters;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Interfaces.Services;
using TechCraftsmen.User.Core.Mapping;
using TechCraftsmen.User.Core.Utils;
using Results = TechCraftsmen.User.Core.Enums.Results;

namespace TechCraftsmen.User.Services
{
    public class UserService(
        ICrudRepository<Core.Entities.User> userRepository,
        IHttpContextAccessor httpContextAccessor,
        IValidator<UserDto> userValidator
    ) : IUserService
    {
        public OperationResultDto<int> CreateUser(UserDto userDto)
        {
            ValidationResult? validationResult = userValidator.Validate(userDto);
            
            string[] validationErrors = validationResult.Errors.Select(vf => vf.ErrorMessage).ToArray();

            if (!validationResult.IsValid)
            {
                return new OperationResultDto<int>(default, validationErrors,
                    Results.ValidationError);
            }

            Dictionary<string, object> filter = new() { { "Email", userDto.Email } };

            IQueryable<Core.Entities.User> userSearch = userRepository.GetByFilter(filter);

            if (userSearch.Any())
            {
                return new OperationResultDto<int>(default, ["E-mail already registered"], Results.NotAllowed);
            }

            Core.Entities.User user = userDto.ToEntity();

            httpContextAccessor.HttpContext!.Items.TryGetValue("User", out object? userData);

            UserDto? authenticatedUser = userData as UserDto;

            SimpleResultDto canRegister = user.CanRegister(authenticatedUser!.RoleId);

            if (!canRegister.Success)
            {
                return new OperationResultDto<int>(default, canRegister.Errors.ToArray(), Results.NotAllowed);
            }

            HashDto hashResult = HashUtils.HashText(userDto.Password);

            user.Password = hashResult.Hash;
            user.Salt = hashResult.Salt;

            int createdId = userRepository.Create(user);

            return new OperationResultDto<int>(createdId, ["User created with success"], Results.Created);
        }

        public OperationResultDto<IList<UserDto>> GetUsersByFilter(UserFilter filter)
        {
            List<Core.Entities.User> users = userRepository.GetByFilter(filter.NonNullPropertiesToDictionary()).ToList();

            if (users.Count == 0)
            {
                return new OperationResultDto<IList<UserDto>>([], ["No users found for the given filter"], Results.NotFound);
            }

            List<UserDto> usersFound = users.Select(user => user.ToDto()).ToList();

            return new OperationResultDto<IList<UserDto>>(usersFound, ["Search completed with success"]);
        }

        public OperationResultDto<HashDto?> GetPasswordByUserId(int id)
        {
            Core.Entities.User? user = userRepository.GetByFilter(id.ToDictionary("Id")).FirstOrDefault();

            return user is null
                ? new OperationResultDto<HashDto?>(default, ["User not found"], Results.NotFound)
                : new OperationResultDto<HashDto?>(new HashDto() { Hash = user.Password, Salt = user.Salt }, ["Password found"]);
        }

        public OperationResultDto<UserDto?> UpdateUser(UserDto userDto)
        {
            Core.Entities.User? currentUser = userRepository.GetByFilter(userDto.Id.ToDictionary("Id")).FirstOrDefault();
            
            if (currentUser == null)
            {
                return new OperationResultDto<UserDto?>(default, ["User not found"], Results.NotFound);
            }

            SimpleResultDto canUpdate = currentUser.CanUpdate();

            if (!canUpdate.Success)
            {
                return new OperationResultDto<UserDto?>(default, canUpdate.Errors.ToArray(), Results.NotAllowed);
            }

            Core.Entities.User user = userDto.ToEntity();

            MergeUser(user, currentUser);

            userRepository.Update(user);

            return new OperationResultDto<UserDto?>(user.ToDto(), ["User updated with success"]);
        }

        public OperationResultDto<int> ActivateUser(int id)
        {
            Core.Entities.User? user = userRepository.GetByFilter(id.ToDictionary("Id")).FirstOrDefault();
            
            if (user == null)
            {
                return new OperationResultDto<int>(default, ["User not found"], Results.NotFound);
            }

            SimpleResultDto canActivate = user.CanActivate();

            if (!canActivate.Success)
            {
                return new OperationResultDto<int>(id, canActivate.Errors.ToArray(), Results.EntityNotChanged);
            }

            user.Active = true;

            userRepository.Update(user);

            return new OperationResultDto<int>(id, ["User activated with success"]);
        }

        public OperationResultDto<int> DeactivateUser(int id)
        {
            Core.Entities.User? user = userRepository.GetByFilter(id.ToDictionary("Id")).FirstOrDefault();
            
            if (user == null)
            {
                return new OperationResultDto<int>(default, ["User not found"], Results.NotFound);
            }

            SimpleResultDto canDeactivate = user.CanDeactivate();

            if (!canDeactivate.Success)
            {
                return new OperationResultDto<int>(id, canDeactivate.Errors.ToArray(), Results.EntityNotChanged);
            }

            user.Active = false;

            userRepository.Update(user);
            
            return new OperationResultDto<int>(id, ["User deactivated with success"]);
        }

        public OperationResultDto<int> DeleteUser(int id)
        {
            Core.Entities.User? user = userRepository.GetByFilter(id.ToDictionary("Id")).FirstOrDefault();
            
            if (user == null)
            {
                return new OperationResultDto<int>(default, ["User not found"], Results.NotFound);
            }

            SimpleResultDto canDelete = user.CanDelete();

            if (!canDelete.Success)
            {
                return new OperationResultDto<int>(id, canDelete.Errors.ToArray(), Results.NotAllowed);
            }

            userRepository.Delete(user);
            
            return new OperationResultDto<int>(id, ["User deleted with success"]);
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
