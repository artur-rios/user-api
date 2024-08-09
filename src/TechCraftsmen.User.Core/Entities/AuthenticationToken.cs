namespace TechCraftsmen.User.Core.Entities
{
    public class AuthenticationToken
    {
        public string? AccessToken { get; init; }
        public bool Authenticated { get; init; }
        public string? Created { get; set; }
        public string? Expiration { get; set; }
    }
}
