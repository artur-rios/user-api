using TechCraftsmen.User.Core.Rules.Email;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Rules.Email
{
    public class EmailRuleTests
    {
        private readonly EmailRule _rule = new();

        private static readonly string[] _invalidEmails = [
            " ",
            "-------",
            "@majjf.com",
            "A@b@c@example.com",
            "Abc.example.com",
            "js@proseware..com",
            "ma@@jjf.com",
            "ma@jjf.",
            "ma@jjf..com",
            "ma@jjf.c",
            "ma_@jjf",
            "ma_@jjf.",
            "j@proseware.com9",
            "js@proseware.com9",
            "ma@hostname.comcom",
            "MA@hostname.coMCom"
        ];

        private static readonly string[] _validEmails = [
            "ma_@jjf.com",
            "12@hostname.com",
            "d.j@server1.proseware.com",
            "david.jones@proseware.com",
            "j.s@server1.proseware.com",
            "jones@ms1.proseware.com",
            "m.a@hostname.co",
            "m_a1a@hostname.com",
            "ma.h.saraf.onemore@hostname.com.edu",
            "ma@hostname.com",
            "ma12@hostname.com",
            "ma-a.aa@hostname.com.edu",
            "ma-a@hostname.com",
            "ma-a@hostname.com.edu",
            "ma-a@1hostname.com",
            "ma.a@1hostname.com",
            "ma@1hostname.com"
        ];

        public static TheoryData<string> InvalidEmails => new(_invalidEmails);

        public static TheoryData<string> ValidEmails => new(_validEmails);

        [Theory]
        [Unit("EmailRule")]
        [MemberData(nameof(InvalidEmails))]
        public void Should_ReturnFalse_For_InvalidEmails(string email)
        {
            var result = _rule.Execute(email);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Email should be valid", result.Errors.FirstOrDefault());
        }

        [Theory]
        [Unit("EmailRule")]
        [InlineData("")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_ReturnFalse_For_NullOrEmptyEmail(string email)
        {
            var result = _rule.Execute(email);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Parameter null or empty", result.Errors.FirstOrDefault());
        }

        [Theory]
        [Unit("EmailRule")]
        [MemberData(nameof(ValidEmails))]
        public void Should_ReturnTrue_For_ValidEmail(string email)
        {
            var result = _rule.Execute(email);

            Assert.True(result.Success);
            Assert.False(result.Errors.Any());
        }
    }
}
