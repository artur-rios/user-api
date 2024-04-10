using TechCraftsmen.User.Core.Configuration;

namespace TechCraftsmen.User.Tests.Utils.Mock
{
    public class AuthenticationTokenConfigurationGenerator
    {
        private string? _audience;
        private string? _issuer;
        private int _seconds;
        private string? _secret;

        public AuthenticationTokenConfigurationGenerator WithAudience(string? audience)
        {
            _audience = audience;

            return this;
        }

        public AuthenticationTokenConfigurationGenerator WithIssuer(string? issuer)
        {
            _issuer = issuer;

            return this;
        }

        public AuthenticationTokenConfigurationGenerator WithSeconds(int seconds)
        {
            _seconds = seconds;

            return this;
        }

        public AuthenticationTokenConfigurationGenerator WithSecret(string? secret)
        {
            _secret = secret;

            return this;
        }

        public AuthenticationTokenConfiguration Generate()
        {
            return new AuthenticationTokenConfiguration()
            {
                Audience = _audience,
                Issuer = _issuer,
                Seconds = _seconds,
                Secret = _secret
            };
        }
    }
}
