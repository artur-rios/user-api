namespace TechCraftsmen.User.Services.Dto
{
    public class AuthenticationCredentialsDto
    {
        public AuthenticationCredentialsDto() { }

        public AuthenticationCredentialsDto(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
