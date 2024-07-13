namespace TechCraftsmen.User.Core.Dto
{
    public class AuthenticationCredentialsDto
    {
        public AuthenticationCredentialsDto() { }

        public AuthenticationCredentialsDto(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
