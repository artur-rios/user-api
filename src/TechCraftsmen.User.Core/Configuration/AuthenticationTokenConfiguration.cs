namespace TechCraftsmen.User.Core.Configuration
{
    public class AuthenticationTokenConfiguration
    {
        public string? Audience { get; init; }
        public string? Issuer { get; init; }
        public int Seconds { get; init; }
        public string? Secret { get; init; }
    }
}
