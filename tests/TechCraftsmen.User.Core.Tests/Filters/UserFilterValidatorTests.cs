using Microsoft.Extensions.Primitives;
using System.Globalization;
using TechCraftsmen.User.Core.Enums;
using TechCraftsmen.User.Core.Extensions;
using TechCraftsmen.User.Core.Filters;
using TechCraftsmen.User.Tests.Utils.Generators;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Filters
{

    public class UserFilterValidatorTests
    {
        private readonly UserFilterValidator _userFilterValidator;
        private readonly RandomStringGenerator _randomStringGenerator;

        private readonly Core.Entities.User _testUser;

        private const string NAME_FILTER = "Name";
        private const string EMAIL_FILTER = "Email";
        private const string ROLE_ID_FILTER = "RoleId";
        private const string CREATED_AT_FILTER = "CreatedAt";
        private const string ACTIVE_FILTER = "Active";

        private readonly string[] _filters = [NAME_FILTER, EMAIL_FILTER, ROLE_ID_FILTER, CREATED_AT_FILTER, ACTIVE_FILTER];

        private readonly Dictionary<string, string> _testFilters = new()
        {
            { NAME_FILTER, "Jhon Doe" },
            { EMAIL_FILTER, "john.doe@mail.com" },
            { ROLE_ID_FILTER, "2" },
            { CREATED_AT_FILTER, DateTime.UtcNow.AddDays(-1).ToString(CultureInfo.InvariantCulture) },
            { ACTIVE_FILTER, "true" }
        };

        public UserFilterValidatorTests()
        {
            _userFilterValidator = new UserFilterValidator();
            _randomStringGenerator = new RandomStringGenerator();
            UserGenerator userGenerator = new();

            _testUser = userGenerator.WithRandomId().WithDefaultEmail().WithDefaultName().Generate();
        }

        [Fact]
        [Unit("UserFilterValidator")]
        public void Should_HaveFilters()
        {
            Assert.True(_filters.All(filterName => _userFilterValidator.Filters.Any(filter => filter.Name == filterName)));
        }

        [Fact]
        [Unit("UserFilterValidator")]
        public void Should_NotParseAndValidate_Filter_WithInvalidName()
        {
            var invalidFilter = new KeyValuePair<string, StringValues>(_randomStringGenerator.WithLength(5).WithLowerChars().Generate(), _randomStringGenerator.WithLength(5).WithLowerChars().Generate());

            var filter = _userFilterValidator.ParseAndValidateFilter(invalidFilter);

            Assert.Null(filter);
        }

        [Fact]
        [Unit("UserFilterValidator")]
        public void Should_NotParseAndValidate_NameFilter_WithInvalidValue()
        {
            var nameFilter = new KeyValuePair<string, StringValues>(NAME_FILTER, string.Empty);

            var filter = _userFilterValidator.ParseAndValidateFilter(nameFilter);

            Assert.Null(filter);
        }

        [Fact]
        [Unit("UserFilterValidator")]
        public void Should_ParseAndValidate_NameFilter()
        {
            var nameFilter = new KeyValuePair<string, StringValues>(NAME_FILTER, _testUser.Name);

            var filter = _userFilterValidator.ParseAndValidateFilter(nameFilter);

            Assert.NotNull(filter);
            Assert.True(filter.Value.Key == NAME_FILTER);
            Assert.True((string)filter.Value.Value == _testUser.Name);
        }

        [Fact]
        [Unit("UserFilterValidator")]
        public void Should_NotParseAndValidate_EmailFilter_WithInvalidValue()
        {
            var emailFilter = new KeyValuePair<string, StringValues>(EMAIL_FILTER, string.Empty);

            var filter = _userFilterValidator.ParseAndValidateFilter(emailFilter);

            Assert.Null(filter);
        }

        [Fact]
        [Unit("UserFilterValidator")]
        public void Should_ParseAndValidate_EmailFilter()
        {
            var emailFilter = new KeyValuePair<string, StringValues>(NAME_FILTER, _testUser.Email);

            var filter = _userFilterValidator.ParseAndValidateFilter(emailFilter);

            Assert.NotNull(filter);
            Assert.True(filter.Value.Key == NAME_FILTER);
            Assert.True((string)filter.Value.Value == _testUser.Email);
        }

        [Fact]
        [Unit("UserFilterValidator")]
        public void Should_NotParseAndValidate_RoleIdFilter_WithInvalidValue()
        {
            var roleIdFilter = new KeyValuePair<string, StringValues>(ROLE_ID_FILTER, _randomStringGenerator.WithLength(5).WithLowerChars().Generate());

            var filter = _userFilterValidator.ParseAndValidateFilter(roleIdFilter);

            Assert.Null(filter);
        }

        [Fact]
        [Unit("UserFilterValidator")]
        public void Should_NotParseAndValidate_RoleIdFilter_WithInvalidRoleId()
        {
            var roleMaxValue = Enum.GetValues(typeof(Roles)).Cast<int>().Max();

            var roleIdFilter = new KeyValuePair<string, StringValues>(ROLE_ID_FILTER, CustomRandomNumberGenerator.RandomWeakIntOnRange(roleMaxValue + 1, roleMaxValue + 10).ToString());

            var filter = _userFilterValidator.ParseAndValidateFilter(roleIdFilter);

            Assert.Null(filter);
        }

        [Fact]
        [Unit("UserFilterValidator")]
        public void Should_ParseAndValidate_RoleIdFilter()
        {
            var roleIdFilter = new KeyValuePair<string, StringValues>(ROLE_ID_FILTER, ((int)Roles.REGULAR).ToString());

            var filter = _userFilterValidator.ParseAndValidateFilter(roleIdFilter);

            Assert.NotNull(filter);
            Assert.True(filter.Value.Key == ROLE_ID_FILTER);
            Assert.True(filter.Value.Value is (int)Roles.REGULAR);
        }

        [Fact]
        [Unit("UserFilterValidator")]
        public void Should_NotParseAndValidate_CreatedAtFilter_WithInvalidValue()
        {
            var createdAtFilter = new KeyValuePair<string, StringValues>(CREATED_AT_FILTER, _randomStringGenerator.WithLength(5).WithLowerChars().Generate());

            var filter = _userFilterValidator.ParseAndValidateFilter(createdAtFilter);

            Assert.Null(filter);
        }

        [Fact]
        [Unit("UserFilterValidator")]
        public void Should_NotParseAndValidate_CreatedAtFilter_WithInvalidDate()
        {
            var createdAtFilter = new KeyValuePair<string, StringValues>(CREATED_AT_FILTER, DateTime.UtcNow.AddDays(1).ToString(CultureInfo.InvariantCulture));

            var filter = _userFilterValidator.ParseAndValidateFilter(createdAtFilter);

            Assert.Null(filter);
        }

        [Fact]
        [Unit("UserFilterValidator")]
        public void Should_ParseAndValidate_CreatedAtFilter()
        {
            var testDateTime = DateTime.UtcNow.AddDays(-1);

            var createdAtFilter = new KeyValuePair<string, StringValues>(CREATED_AT_FILTER, testDateTime.ToString(CultureInfo.InvariantCulture));

            var filter = _userFilterValidator.ParseAndValidateFilter(createdAtFilter);

            Assert.NotNull(filter);
            Assert.True(filter.Value.Key == CREATED_AT_FILTER);
            Assert.True(filter.Value.Value is DateTime dateTimeValue && dateTimeValue.TrimMilliseconds() == testDateTime.TrimMilliseconds());
        }

        [Fact]
        [Unit("UserFilterValidator")]
        public void Should_NotParseAndValidate_ActiveFilter_WithInvalidValue()
        {
            var activeFilter = new KeyValuePair<string, StringValues>(ACTIVE_FILTER, _randomStringGenerator.WithLength(5).WithLowerChars().Generate());

            var filter = _userFilterValidator.ParseAndValidateFilter(activeFilter);

            Assert.Null(filter);
        }

        [Fact]
        [Unit("UserFilterValidator")]
        public void Should_ParseAndValidate_ActiveFilter()
        {
            var activeFilter = new KeyValuePair<string, StringValues>(ACTIVE_FILTER, true.ToString());

            var filter = _userFilterValidator.ParseAndValidateFilter(activeFilter);

            Assert.NotNull(filter);
            Assert.True(filter.Value.Key == ACTIVE_FILTER);
            Assert.True((bool)filter.Value.Value);
        }

        [Fact]
        [Unit("UserFilterValidator")]
        public void Should_NotParseAndValidate_FiltersWithInvalidNames()
        {
            var nameFilter = _testFilters.First(filter => filter.Key == NAME_FILTER);
            var emailFilter = _testFilters.First(filter => filter.Key == EMAIL_FILTER);

            Dictionary<string, string> filtersToTest = new()
            {
                { nameFilter.Key, nameFilter.Value },
                { emailFilter.Key, emailFilter.Value },
                { _randomStringGenerator.WithLength(5).WithLowerChars().Generate(), nameFilter.Value }
            };

            var validFilters = _userFilterValidator.ParseAndValidateFilters(filtersToTest.ToQueryCollection());

            Assert.NotNull(validFilters);
            Assert.True(validFilters.Count == filtersToTest.Count - 1);
        }

        [Fact]
        [Unit("UserFilterValidator")]
        public void Should_NotParseAndValidate_FiltersWithInvalidValues()
        {
            var nameFilter = _testFilters.First(filter => filter.Key == NAME_FILTER);
            var emailFilter = _testFilters.First(filter => filter.Key == EMAIL_FILTER);

            Dictionary<string, string> filtersToTest = new()
            {
                { nameFilter.Key, nameFilter.Value },
                { emailFilter.Key, emailFilter.Value },
                { CREATED_AT_FILTER, DateTime.UtcNow.AddDays(1).ToString(CultureInfo.InvariantCulture) }
            };

            var validFilters = _userFilterValidator.ParseAndValidateFilters(filtersToTest.ToQueryCollection());

            Assert.NotNull(validFilters);
            Assert.True(validFilters.Count == filtersToTest.Count - 1);
        }

        [Theory]
        [Unit("UserFilterValidator")]
        [InlineData(NAME_FILTER)]
        [InlineData(NAME_FILTER, EMAIL_FILTER)]
        [InlineData(NAME_FILTER, EMAIL_FILTER, ROLE_ID_FILTER)]
        [InlineData(NAME_FILTER, EMAIL_FILTER, ROLE_ID_FILTER, CREATED_AT_FILTER)]
        [InlineData(NAME_FILTER, EMAIL_FILTER, ROLE_ID_FILTER, CREATED_AT_FILTER, ACTIVE_FILTER)]
        public void Should_ParseAndValidate_Filters(params string[] filters)
        {
            Dictionary<string, string> filtersToTest = [];

            foreach (var filter in filters)
            {
                var filterToTest = _testFilters.First(f => f.Key == filter);

                filtersToTest.Add(filterToTest.Key, filterToTest.Value);
            }

            var validFilters = _userFilterValidator.ParseAndValidateFilters(filtersToTest.ToQueryCollection());

            Assert.NotNull(validFilters);
            Assert.True(validFilters.Count == filters.Length);

            var validFilterCounter = 0;

            foreach (var filter in filtersToTest)
            {
                if (validFilters.Any(f => f.Key == filter.Key && f.Value is not null))
                {
                    validFilterCounter++;
                }
            }

            Assert.True(validFilterCounter == filters.Length);
        }
    }
}
