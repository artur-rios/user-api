namespace TechCraftsmen.User.Core.Configuration
{
    public class AuthenticationTokenConfiguration
    {
        public string? Audience { get; set; }
        public string? Issuer { get; set; }
        public int Seconds { get; set; }
        public string? Secret { get; set; }
    }
}
