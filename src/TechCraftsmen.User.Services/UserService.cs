using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Interfaces.Repositories;
using TechCraftsmen.User.Core.Interfaces.Services;
using TechCraftsmen.User.Core.Rules.User;
using TechCraftsmen.User.Core.Utils;

namespace TechCraftsmen.User.Core.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ICrudRepository<Entities.User> _userRepository;
        private readonly IValidator<UserDto> _userValidator;

        private readonly UserCreationRule _creationRule;
        private readonly UserUpdateRule _updateRule;
        private readonly UserStatusUpdateRule _statusUpdateRule;
        private readonly UserDeletionRule _deletionRule;

        private readonly Dictionary<string, Func<string, object>> _filterParsers = new()
        {
            { "Name", value => value },
            { "Email", value => value },
            { "RoleId", value => value.ToInt()  },
            { "CreateAt", value => value.ToDateTime() },
            { "Active", value => value }
        };

        private readonly Dictionary<string, Func<object, bool>> _filterValidators = new()
        {
            { "Name", value => value is string && !string.IsNullOrEmpty(value as string) },
            { "Email", value => value is string && !string.IsNullOrEmpty(value as string) },
            { "RoleId", value => value is int && (int) value > 0 },
            { "CreateAt", value => value is DateTime && (DateTime) value < DateTime.UtcNow },
            { "Active", value => value is bool }
        };

        public UserService(IMapper mapper, ICrudRepository<Entities.User> userRepository, IValidator<UserDto> userValidator, UserCreationRule creationRule, UserUpdateRule updateRule, UserStatusUpdateRule statusUpdateRule, UserDeletionRule deletionRule)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _userValidator = userValidator;
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

        public IEnumerable<UserDto> GetUsersByFilter(IQueryCollection query)
        {
            Dictionary<string, object> filters = ParseQueryParams(query);
            Dictionary<string, object> validFilters = [];

            if (filters.Count > 0)
            {
                foreach (var filter in filters)
                {
                    var filterValidator = _filterValidators.GetValueOrDefault(filter.Key);

                    if (filterValidator is not null)
                    {
                        if (filterValidator(filter.Value))
                        {
                            validFilters.Add(filter.Key, filter.Value);
                        }
                    }
                }
            }

            if (validFilters.Count == 0)
            {
                throw new NotAllowedException("No valid filters were passed!");
            }

            return _userRepository.GetByFilter(validFilters) as IEnumerable<UserDto> ?? throw new NotFoundException("No users found with the given filter");
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

            var user = _mapper.Map<Entities.User>(userDto);

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

        private static void MergeUser(Entities.User source, Entities.User target, bool mergeStatus = true)
        {
            target.Password = source.Password;
            target.CreatedAt = source.CreatedAt;

            if (mergeStatus)
            {
                target.Active = source.Active;
            }
        }

        private Dictionary<string, object> ParseQueryParams(IQueryCollection query)
        {
            var parsedFilters = new Dictionary<string, object>();

            if (query is not null || query?.Count > 0)
            {
                foreach (var item in query)
                {
                    var filterParser = _filterParsers.GetValueOrDefault(item.Key);

                    if (filterParser is not null)
                    {
                        var parsedFilter = filterParser(item.Value);

                        if (parsedFilter is not null)
                        {
                            parsedFilters.Add(item.Key, parsedFilter);
                        }
                    }
                }
            }

            return parsedFilters;
        }
    }
}
