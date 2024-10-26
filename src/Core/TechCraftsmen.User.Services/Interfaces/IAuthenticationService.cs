using TechCraftsmen.User.Services.Authentication;
using TechCraftsmen.User.Services.Dto;
using TechCraftsmen.User.Services.Output;

namespace TechCraftsmen.User.Services.Interfaces
{
    public interface IAuthenticationService
    {
        ServiceOutput<AuthenticationToken?> AuthenticateUser(AuthenticationCredentialsDto credentialsDto);
        ServiceOutput<bool> ValidateJwtToken(string token, out UserDto? authenticatedUser);
    }
}
