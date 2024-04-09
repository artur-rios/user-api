using TechCraftsmen.User.Core.Rules.User;
using TechCraftsmen.User.Core.Utils;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Rules.User
{
    public class UserStatusUpdateRuleTests
    {
        public static TheoryData<Tuple<bool, bool>> EqualUserStatusTestData => [new(false, false), new(true, true)];
        public static TheoryData<Tuple<bool, bool>> DifferentUserStatusTestData => [new(false, true), new(true, false)];

        private readonly UserStatusUpdateRule _rule = new();

        [Theory]
        [Unit("UserStatusUpdateRule")]
        [MemberData(nameof(EqualUserStatusTestData))]
        public void Should_ReturnFalse_ForSameStatuses(Tuple<bool, bool> statuses)
        {
            var result = _rule.Execute(statuses);

            statuses.Deconstruct(out bool actualStatus, out _);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal($"User already {actualStatus.ToCustomString("active", "inactive")}", result.Errors.FirstOrDefault());
        }

        [Theory]
        [Unit("UserStatusUpdateRule")]
        [MemberData(nameof(DifferentUserStatusTestData))]
        public void Should_ReturnTrue_ForDifferentStatuses(Tuple<bool, bool> statuses)
        {
            var result = _rule.Execute(statuses);

            Assert.True(result.Success);
            Assert.False(result.Errors.Any());
        }
    }
}
