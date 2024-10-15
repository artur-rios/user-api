using TechCraftsmen.User.Core.Entities;
using TechCraftsmen.User.Core.ValueObjects;
using TechCraftsmen.User.Services.Dto;

namespace TechCraftsmen.User.Services.Interfaces
{
    public interface IAuthenticationService
    {
        ServiceOutput<AuthenticationToken?> AuthenticateUser(AuthenticationCredentialsDto credentialsDto);
        ServiceOutput<bool> ValidateJwtToken(string token, out UserDto? authenticatedUser);
    }
}
