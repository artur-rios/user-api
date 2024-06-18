using Microsoft.Extensions.Primitives;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Extensions;
using TechCraftsmen.User.Core.Filters;
using TechCraftsmen.User.Tests.Utils.Mock;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Filters
{

    public class UserFilterValidatorTests
    {
        private readonly UserFilterValidator _userFilterValidator;
        private readonly RandomStringGenerator _randomStringGenerator;
        private readonly UserGenerator _userGenerator;

        private readonly Entities.User _testUser;

        private const string NAME_FILTER = "Name";
        private const string EMAIL_FILTER = "Email";
        private const string ROLE_ID_FILTER = "RoleId";
        private const string CREATED_AT_FILTER = "CreatedAt";
        private const string ACTIVE_FILTER = "Active";

        private readonly string[] _filters = [NAME_FILTER, EMAIL_FILTER, ROLE_ID_FILTER, CREATED_AT_FILTER, ACTIVE_FILTER];

        public UserFilterValidatorTests()
        {
            _userFilterValidator = new UserFilterValidator();
            _randomStringGenerator = new RandomStringGenerator();
            _userGenerator = new UserGenerator();

            _testUser = _userGenerator.WithRandomId().WithDefaultEmail().WithDefaultName().Generate();
        }

        [Fact]
        public void Should_HaveFilters()
        {
            Assert.True(_filters.All(filterName => _userFilterValidator.Filters.Any(filter => filter.Name == filterName)));
        }

        [Fact]
        public void Should_NotParseAndValidate_Filter_WithInvalidName()
        {
            var invalidFilter = new KeyValuePair<string, StringValues>(_randomStringGenerator.WithLength(5).WithLowerChars().Generate(), _randomStringGenerator.WithLength(5).WithLowerChars().Generate());

            var filter = _userFilterValidator.ParseAndValidateFilter(invalidFilter);

            Assert.Null(filter);
        }

        [Fact]
        public void Should_NotParseAndValidate_NameFilter_WithInvalidValue()
        {
            var nameFilter = new KeyValuePair<string, StringValues>(NAME_FILTER, string.Empty);

            var filter = _userFilterValidator.ParseAndValidateFilter(nameFilter);

            Assert.Null(filter);
        }

        [Fact]
        public void Should_ParseAndValidate_NameFilter()
        {
            var nameFilter = new KeyValuePair<string, StringValues>(NAME_FILTER, _testUser.Name);

            var filter = _userFilterValidator.ParseAndValidateFilter(nameFilter);

            Assert.NotNull(filter);
            Assert.True(filter.Value.Key == NAME_FILTER);
            Assert.True((string)filter.Value.Value == _testUser.Name);
        }

        [Fact]
        public void Should_NotParseAndValidate_EmailFilter_WithInvalidValue()
        {
            var emailFilter = new KeyValuePair<string, StringValues>(EMAIL_FILTER, string.Empty);

            var filter = _userFilterValidator.ParseAndValidateFilter(emailFilter);

            Assert.Null(filter);
        }

        [Fact]
        public void Should_ParseAndValidate_EmailFilter()
        {
            var emailFilter = new KeyValuePair<string, StringValues>(NAME_FILTER, _testUser.Email);

            var filter = _userFilterValidator.ParseAndValidateFilter(emailFilter);

            Assert.NotNull(filter);
            Assert.True(filter.Value.Key == NAME_FILTER);
            Assert.True((string)filter.Value.Value == _testUser.Email);
        }

        [Fact]
        public void Should_NotParseAndValidate_RoleIdFilter_WithInvalidValue()
        {
            var roleIdFilter = new KeyValuePair<string, StringValues>(ROLE_ID_FILTER, _randomStringGenerator.WithLength(5).WithLowerChars().Generate());

            var filter = _userFilterValidator.ParseAndValidateFilter(roleIdFilter);

            Assert.Null(filter);
        }

        [Fact]
        public void Should_NotParseAndValidate_RoleIdFilter_WithInvalidRoleId()
        {
            var roleMaxValue = Enum.GetValues(typeof(Roles)).Cast<int>().Max();

            var roleIdFilter = new KeyValuePair<string, StringValues>(ROLE_ID_FILTER, CustomRandomNumberGenerator.RandomWeakIntOnRange(roleMaxValue++, roleMaxValue + 10).ToString());

            var filter = _userFilterValidator.ParseAndValidateFilter(roleIdFilter);

            Assert.Null(filter);
        }

        [Fact]
        public void Should_ParseAndValidate_RoleIdFilter()
        {
            var roleIdFilter = new KeyValuePair<string, StringValues>(ROLE_ID_FILTER, ((int)Roles.REGULAR).ToString());

            var filter = _userFilterValidator.ParseAndValidateFilter(roleIdFilter);

            Assert.NotNull(filter);
            Assert.True(filter.Value.Key == ROLE_ID_FILTER);
            Assert.True(filter.Value.Value is int intValue && intValue == (int)Roles.REGULAR);
        }

        [Fact]
        public void Should_NotParseAndValidate_CreatedAtFilter_WithInvalidValue()
        {
            var createdAtFilter = new KeyValuePair<string, StringValues>(CREATED_AT_FILTER, _randomStringGenerator.WithLength(5).WithLowerChars().Generate());

            var filter = _userFilterValidator.ParseAndValidateFilter(createdAtFilter);

            Assert.Null(filter);
        }

        [Fact]
        public void Should_NotParseAndValidate_CreatedAtFilter_WithInvalidDate()
        {
            var createdAtFilter = new KeyValuePair<string, StringValues>(CREATED_AT_FILTER, DateTime.UtcNow.AddDays(1).ToString());

            var filter = _userFilterValidator.ParseAndValidateFilter(createdAtFilter);

            Assert.Null(filter);
        }

        [Fact]
        public void Should_ParseAndValidate_CreatedAtFilter()
        {
            var testDateTime = DateTime.UtcNow.AddDays(-1);

            var createdAtFilter = new KeyValuePair<string, StringValues>(CREATED_AT_FILTER, testDateTime.ToString());

            var filter = _userFilterValidator.ParseAndValidateFilter(createdAtFilter);

            Assert.NotNull(filter);
            Assert.True(filter.Value.Key == CREATED_AT_FILTER);
            Assert.True(filter.Value.Value is DateTime dateTimeValue && dateTimeValue.TrimMilliseconds() == testDateTime.TrimMilliseconds());
        }

        [Fact]
        public void Should_NotParseAndValidate_ActiveFilter_WithInvalidValue()
        {
            var activeFilter = new KeyValuePair<string, StringValues>(ACTIVE_FILTER, _randomStringGenerator.WithLength(5).WithLowerChars().Generate());

            var filter = _userFilterValidator.ParseAndValidateFilter(activeFilter);

            Assert.Null(filter);
        }

        [Fact]
        public void Should_ParseAndValidate_ActiveFilter()
        {
            var activeFilter = new KeyValuePair<string, StringValues>(ACTIVE_FILTER, true.ToString());

            var filter = _userFilterValidator.ParseAndValidateFilter(activeFilter);

            Assert.NotNull(filter);
            Assert.True(filter.Value.Key == ACTIVE_FILTER);
            Assert.True((bool)filter.Value.Value);
        }
    }
}
