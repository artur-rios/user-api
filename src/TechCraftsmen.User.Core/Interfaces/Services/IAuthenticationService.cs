using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Entities;

namespace TechCraftsmen.User.Core.Interfaces.Services
{
    public interface IAuthenticationService
    {
        AuthenticationToken AuthenticateUser(AuthenticationCredentialsDto credentialsDto);
        AuthenticationToken GenerateJwtToken(int userId);
        int GetUserIdFromJwtToken(string token);
        bool ValidateJwtToken(string token, out UserDto? authenticatedUser);
    }
}
