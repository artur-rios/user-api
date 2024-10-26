using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using TechCraftsmen.User.Domain.Interfaces;
using TechCraftsmen.User.Services.Dto;
using TechCraftsmen.User.Services.Filters;
using TechCraftsmen.User.Services.Interfaces;
using TechCraftsmen.User.Services.Mapping;
using TechCraftsmen.User.Services.Output;
using TechCraftsmen.User.Utils.Extensions;
using TechCraftsmen.User.Utils.Security;
using Results = TechCraftsmen.User.Services.Enums.Results;

namespace TechCraftsmen.User.Services.Implementations
{
    public class UserService(
        ICrudRepository<Domain.Entities.User> userRepository,
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

            IQueryable<Domain.Entities.User> userSearch = userRepository.GetByFilter(filter, true);

            if (userSearch.Any())
            {
                return new ServiceOutput<int>(default, ["E-mail already registered"], Results.NotAllowed);
            }

            Domain.Entities.User user = userDto.ToEntity();

            httpContextAccessor.HttpContext!.Items.TryGetValue("User", out object? userData);
            
            UserDto? authenticatedUser = userData as UserDto;
            
            if (!user.CanRegister(authenticatedUser!.RoleId, out string[] errors))
            {
                return new ServiceOutput<int>(default, errors.ToArray(), Results.NotAllowed);
            }

            Hash hash = new(userDto.Password);

            user.Password = hash.Value;
            user.Salt = hash.Salt;

            int createdId = userRepository.Create(user);

            return new ServiceOutput<int>(createdId, ["User created with success"], Results.Created);
        }

        public ServiceOutput<IList<UserDto>> GetUsersByFilter(UserFilter filter)
        {
            List<Domain.Entities.User> users = userRepository.GetByFilter(filter.NonNullPropertiesToDictionary(), false).ToList();

            if (users.Count == 0)
            {
                return new ServiceOutput<IList<UserDto>>([], ["No users found for the given filter"], Results.NotFound);
            }

            List<UserDto> usersFound = users.Select(user => user.ToDto()).ToList();

            return new ServiceOutput<IList<UserDto>>(usersFound, ["Search completed with success"]);
        }

        public ServiceOutput<UserDto?> UpdateUser(UserDto userDto)
        {
            Domain.Entities.User? currentUser = userRepository.GetByFilter(userDto.Id.ToDictionary("Id"), false).FirstOrDefault();
            
            if (currentUser == null)
            {
                return new ServiceOutput<UserDto?>(default, ["User not found"], Results.NotFound);
            }

            if (!currentUser.CanUpdate(out string[] errors))
            {
                return new ServiceOutput<UserDto?>(default, errors.ToArray(), Results.NotAllowed);
            }

            Domain.Entities.User user = userDto.ToEntity();

            MergeUser(currentUser, user);

            userRepository.Update(user);

            return new ServiceOutput<UserDto?>(user.ToDto(), ["User updated with success"]);
        }

        public ServiceOutput<int> ActivateUser(int id)
        {
            Domain.Entities.User? user = userRepository.GetByFilter(id.ToDictionary("Id"), false).FirstOrDefault();
            
            if (user == null)
            {
                return new ServiceOutput<int>(default, ["User not found"], Results.NotFound);
            }

            if (!user.CanActivate(out string[] errors))
            {
                return new ServiceOutput<int>(id, errors.ToArray(), Results.EntityNotChanged);
            }

            user.Active = true;

            userRepository.Update(user);

            return new ServiceOutput<int>(id, ["User activated with success"]);
        }

        public ServiceOutput<int> DeactivateUser(int id)
        {
            Domain.Entities.User? user = userRepository.GetByFilter(id.ToDictionary("Id"), false).FirstOrDefault();
            
            if (user == null)
            {
                return new ServiceOutput<int>(default, ["User not found"], Results.NotFound);
            }

            if (!user.CanDeactivate(out string[] errors))
            {
                return new ServiceOutput<int>(id, errors.ToArray(), Results.EntityNotChanged);
            }

            user.Active = false;

            userRepository.Update(user);
            
            return new ServiceOutput<int>(id, ["User deactivated with success"]);
        }

        public ServiceOutput<int> DeleteUser(int id)
        {
            Domain.Entities.User? user = userRepository.GetByFilter(id.ToDictionary("Id"), true).FirstOrDefault();
            
            if (user == null)
            {
                return new ServiceOutput<int>(default, ["User not found"], Results.NotFound);
            }

            if (!user.CanDelete(out string[] errors))
            {
                return new ServiceOutput<int>(id, errors.ToArray(), Results.NotAllowed);
            }

            userRepository.Delete(user);
            
            return new ServiceOutput<int>(id, ["User deleted with success"]);
        }

        private static void MergeUser(Domain.Entities.User source, Domain.Entities.User target, bool mergeStatus = true)
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
