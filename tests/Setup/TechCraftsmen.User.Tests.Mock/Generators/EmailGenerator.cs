namespace TechCraftsmen.User.Tests.Mock.Generators
{
    public class EmailGenerator
    {
        private const string DefaultDomainName = "mail.com";
        private const string DefaultUserName = "jhon.doe";

        private string? _customDomainName;
        private string? _customUsername;
        private int? _id;

        public EmailGenerator WithDomainName(string domainName)
        {
            _customDomainName = domainName;

            return this;
        }

        public EmailGenerator WithUserName(string username)
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
            string username = _customUsername ?? DefaultUserName;
            string domainName = _customDomainName ?? DefaultDomainName;

            if (_id is not null)
            {
                username += _id;
            }

            return $"{username}@{domainName}";
        }
    }
}
