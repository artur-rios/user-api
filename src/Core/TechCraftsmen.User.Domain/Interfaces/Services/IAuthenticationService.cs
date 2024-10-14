using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Entities;

namespace TechCraftsmen.User.Core.Interfaces.Services
{
    public interface IAuthenticationService
    {
        OperationResultDto<AuthenticationToken?> AuthenticateUser(AuthenticationCredentialsDto credentialsDto);
        OperationResultDto<bool> ValidateJwtToken(string token, out UserDto? authenticatedUser);
    }
}
