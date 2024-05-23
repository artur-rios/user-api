using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechCraftsmen.User.Core.Configuration;
using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Entities;
using TechCraftsmen.User.Core.Exceptions;
using TechCraftsmen.User.Core.Interfaces.Services;
using TechCraftsmen.User.Core.Utils;


namespace TechCraftsmen.User.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationTokenConfiguration _authTokenConfig;
        private readonly IUserService _userService;
        private readonly IValidator<AuthenticationCredentialsDto> _authCredentialsValidator;
        private readonly IValidator<AuthenticationTokenConfiguration> _authTokenConfigValidator;

        public AuthenticationService(IOptions<AuthenticationTokenConfiguration> authTokenConfig, IValidator<AuthenticationCredentialsDto> authCredentialsValidator, IValidator<AuthenticationTokenConfiguration> authTokenConfigValidator, IUserService userService)
        {
            _authCredentialsValidator = authCredentialsValidator;
            _authTokenConfig = authTokenConfig.Value;
            _authTokenConfigValidator = authTokenConfigValidator;
            _userService = userService;

            _authTokenConfigValidator.ValidateAndThrow(_authTokenConfig);
        }

        public AuthenticationToken AuthenticateUser(AuthenticationCredentialsDto credentialsDto)
        {
            _authCredentialsValidator.ValidateAndThrow(credentialsDto);

            Dictionary<string, StringValues> filter = new()
            {
                { "Email",  credentialsDto.Email },
            };

            var user = _userService.GetUsersByFilter(new QueryCollection(filter)).FirstOrDefault() ?? throw new NotAllowedException("Invalid credentials");

            var password = _userService.GetPasswordByUserId(user.Id);

            if (!HashUtils.VerifyHash(credentialsDto.Password, password))
            {
                throw new NotAllowedException("Invalid credentials");
            }

            return GenerateJwtToken(user.Id);
        }

        public AuthenticationToken GenerateJwtToken(int userId)
        {
            var key = Encoding.ASCII.GetBytes(_authTokenConfig.Secret!);

            ClaimsIdentity identity = new([new Claim("id", userId.ToString())]);

            DateTime creationDate = DateTime.Now;
            DateTime expirationDate = creationDate + TimeSpan.FromSeconds(_authTokenConfig.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _authTokenConfig.Issuer,
                Audience = _authTokenConfig.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = identity,
                NotBefore = creationDate,
                Expires = expirationDate
            });
            var token = handler.WriteToken(securityToken);

            return new()
            {
                Authenticated = true,
                Created = creationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                AccessToken = token
            };
        }

        public int GetUserIdFromJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadToken(token);
            var jwtSecurityToken = jsonToken as JwtSecurityToken;

            return int.Parse(jwtSecurityToken!.Claims.First(x => x.Type == "id").Value);
        }

        public bool ValidateJwtToken(string token, out UserDto? authenticatedUser)
        {
            if (token is null)
            {
                authenticatedUser = null;

                return false;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authTokenConfig.Secret!);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            authenticatedUser = _userService.GetUserById(userId);

            return authenticatedUser is not null;
        }
    }
}
