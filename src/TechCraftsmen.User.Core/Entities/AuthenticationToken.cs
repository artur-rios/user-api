namespace TechCraftsmen.User.Core.Entities
{
    public class AuthenticationToken
    {
        public string? AccessToken { get; set; }
        public bool Authenticated { get; set; }
        public string? Created { get; set; }
        public string? Expiration { get; set; }
    }
}
