namespace TechCraftsmen.User.Tests.Utils.Generators
{
    public class EmailGenerator
    {
        public const string DEFAULT_DOMAIN_NAME = "mail.com";
        public const string DEFAULT_USER_NAME = "jhon.doe";

        private string? _customDomainName = null;
        private string? _customUsername = null;
        private int? _id = null;

        public EmailGenerator WithCustomDomainName(string domainName)
        {
            _customDomainName = domainName;

            return this;
        }

        public EmailGenerator WithCustomUserName(string username)
        {
            _customUsername = username;

            return this;
        }

        public EmailGenerator WithId(int id)
        {
            _id = id;

            return this;
        }

        public EmailGenerator WithRandomId()
        {
            _id = CustomRandomNumberGenerator.RandomWeakIntOnRange(0, 999);

            return this;
        }

        public string Generate()
        {
            var username = _customUsername is null ? DEFAULT_USER_NAME : _customUsername;
            var domainName = _customDomainName is null ? DEFAULT_DOMAIN_NAME : _customDomainName;

            if (_id is not null)
            {
                username += _id;
            }

            return $"{username}@{domainName}";
        }
    }
}
