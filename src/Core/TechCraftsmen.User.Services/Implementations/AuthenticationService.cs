using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechCraftsmen.User.Domain.Interfaces;
using TechCraftsmen.User.Services.Authentication;
using TechCraftsmen.User.Services.Configuration;
using TechCraftsmen.User.Services.Dto;
using TechCraftsmen.User.Services.Enums;
using TechCraftsmen.User.Services.Filters;
using TechCraftsmen.User.Services.Interfaces;
using TechCraftsmen.User.Services.Mapping;
using TechCraftsmen.User.Services.Output;
using TechCraftsmen.User.Utils.Exceptions;
using TechCraftsmen.User.Utils.Extensions;
using TechCraftsmen.User.Utils.Security;

namespace TechCraftsmen.User.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationTokenConfiguration _authTokenConfig;
        private readonly IValidator<AuthenticationCredentialsDto> _authCredentialsValidator;
        private readonly ICrudRepository<Domain.Entities.User> _userRepository;

        public AuthenticationService(
            IOptions<AuthenticationTokenConfiguration> authTokenConfig,
            IValidator<AuthenticationCredentialsDto> authCredentialsValidator,
            IValidator<AuthenticationTokenConfiguration> authTokenConfigValidator,
            ICrudRepository<Domain.Entities.User> userRepository)
        {
            _authCredentialsValidator = authCredentialsValidator;
            _authTokenConfig = authTokenConfig.Value;
            _userRepository = userRepository;

            ValidateTokenConfigAndThrow(authTokenConfigValidator);
        }

        public ServiceOutput<AuthenticationToken?> AuthenticateUser(AuthenticationCredentialsDto credentialsDto)
        {
            ValidationResult? validationResult = _authCredentialsValidator.Validate(credentialsDto);

            string[] validationErrors = validationResult.Errors.Select(vf => vf.ErrorMessage).ToArray();

            if (!validationResult.IsValid)
            {
                return new ServiceOutput<AuthenticationToken?>(default, validationErrors,
                    Results.ValidationError);
            }

            UserFilter searchFilter = new(credentialsDto.Email);

            List<Domain.Entities.User> search = _userRepository.GetByFilter(searchFilter.NonNullPropertiesToDictionary(), false).ToList();

            if (search.Count == 0)
            {
                return new ServiceOutput<AuthenticationToken?>(default, ["Invalid credentials"], Results.NotAllowed);
            }

            Domain.Entities.User user = search.First();

            Hash passwordHash = new Hash(user.Password, user.Salt);

            if (!passwordHash.TextMatches(credentialsDto.Password))
            {
                return new ServiceOutput<AuthenticationToken?>(default, ["Invalid credentials"], Results.NotAllowed);
            }

            AuthenticationToken authToken = GenerateJwtToken(user.Id);

            return new ServiceOutput<AuthenticationToken?>(authToken, ["User authenticated with success"]);
        }

        public ServiceOutput<bool> ValidateJwtToken(string token, out UserDto? authenticatedUser)
        {
            authenticatedUser = default;

            if (string.IsNullOrWhiteSpace(token))
            {
                return new ServiceOutput<bool>(false, ["Invalid auth token"]);
            }

            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(_authTokenConfig.Secret!);

            try
            {
                tokenHandler.ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                JwtSecurityToken? jwtToken = (JwtSecurityToken)validatedToken;

                int userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                
                UserFilter searchFilter = new(userId);

                List<Domain.Entities.User> search = _userRepository.GetByFilter(searchFilter.NonNullPropertiesToDictionary(), false).ToList();

                if (search.Count == 0)
                {
                    return new ServiceOutput<bool>(false, ["User not found"], Results.NotFound);
                }

                authenticatedUser = search.First().ToDto();

                return new ServiceOutput<bool>(true, ["Auth token is valid"]);
            }
            catch (SecurityTokenExpiredException)
            {
                return new ServiceOutput<bool>(false, ["Auth token expired. Please authenticate again"]);
            }
            catch (Exception)
            {
                return new ServiceOutput<bool>(false, ["Auth token invalid"]);
            }
        }

        private AuthenticationToken GenerateJwtToken(int userId)
        {
            byte[] key = Encoding.ASCII.GetBytes(_authTokenConfig.Secret!);

            ClaimsIdentity identity = new([new Claim("id", userId.ToString())]);

            DateTime creationDate = DateTime.Now;
            DateTime expirationDate = creationDate + TimeSpan.FromSeconds(_authTokenConfig.Seconds);

            JwtSecurityTokenHandler handler = new();
            SecurityToken? securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _authTokenConfig.Issuer,
                Audience = _authTokenConfig.Audience,
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = identity,
                NotBefore = creationDate,
                Expires = expirationDate
            });
            string? token = handler.WriteToken(securityToken);

            return new AuthenticationToken
            {
                Authenticated = true,
                Created = creationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                AccessToken = token
            };
        }

        private void ValidateTokenConfigAndThrow(IValidator<AuthenticationTokenConfiguration> authTokenConfigValidator)
        {
            ValidationResult? validationResult = authTokenConfigValidator.Validate(_authTokenConfig);

            if (validationResult.IsValid)
            {
                return;
            }

            string[] validationErrors = validationResult.Errors.Select(vf => vf.ErrorMessage).ToArray();

            throw new CustomException(validationErrors);
        }
    }
}
