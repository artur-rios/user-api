using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechCraftsmen.User.Core.Configuration;
using TechCraftsmen.User.Core.Entities;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Filters;
using TechCraftsmen.User.Core.Utils;
using TechCraftsmen.User.Core.ValueObjects;
using TechCraftsmen.User.Services.Dto;
using TechCraftsmen.User.Services.Interfaces;

namespace TechCraftsmen.User.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationTokenConfiguration _authTokenConfig;
        private readonly IUserService _userService;
        private readonly IValidator<AuthenticationCredentialsDto> _authCredentialsValidator;

        public AuthenticationService(IOptions<AuthenticationTokenConfiguration> authTokenConfig,
            IValidator<AuthenticationCredentialsDto> authCredentialsValidator,
            IValidator<AuthenticationTokenConfiguration> authTokenConfigValidator, IUserService userService)
        {
            _authCredentialsValidator = authCredentialsValidator;
            _authTokenConfig = authTokenConfig.Value;
            _userService = userService;

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

            ServiceOutput<IList<UserDto>> search =
                _userService.GetUsersByFilter(new UserFilter(credentialsDto.Email));

            if (search.Result is not Results.Success)
            {
                return new ServiceOutput<AuthenticationToken?>(default, ["Invalid credentials"], Results.NotAllowed);
            }

            UserDto user = search.Data!.First();

            ServiceOutput<HashOutput?> passwordSearch = _userService.GetPasswordByUserId(user.Id);

            if (passwordSearch.Result is not Results.Success)
            {
                return new ServiceOutput<AuthenticationToken?>(default,
                    ["Could not retrieve password from database"], Results.InternalError);
            }

            HashOutput password = passwordSearch.Data!;

            if (!HashUtils.VerifyHash(credentialsDto.Password, password))
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

                ServiceOutput<IList<UserDto>> userSearch = _userService.GetUsersByFilter(new UserFilter(userId));

                if (userSearch.Result is not Results.Success)
                {
                    return new ServiceOutput<bool>(false, ["User not found"], Results.NotFound);
                }

                authenticatedUser = userSearch.Data!.First();

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