using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Validation;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Validation
{
    public class EmailValidatorTests
    {
        private readonly EmailValidator _validator = new();

        private static readonly string[] InvalidEmails = [
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

        private static readonly string[] ValidEmails = [
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

        public static TheoryData<string> InvalidEmailsData => new(InvalidEmails);

        public static TheoryData<string> ValidEmailsData => new(ValidEmails);

        [Theory]
        [Unit("EmailValidator")]
        [MemberData(nameof(InvalidEmailsData))]
        public void Should_ReturnFalse_For_InvalidEmails(string email)
        {
            SimpleResultDto result = _validator.Validate(email);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Email should be valid", result.Errors.FirstOrDefault());
        }

        [Theory]
        [Unit("EmailValidator")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Testing purposes")]
        public void Should_ReturnFalse_For_NullOrEmptyEmail(string email)
        {
            SimpleResultDto result = _validator.Validate(email);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Email must not be null or empty", result.Errors.FirstOrDefault());
        }

        [Theory]
        [Unit("EmailValidator")]
        [MemberData(nameof(ValidEmailsData))]
        public void Should_ReturnTrue_For_ValidEmail(string email)
        {
            SimpleResultDto result = _validator.Validate(email);

            Assert.True(result.Success);
            Assert.False(result.Errors.Any());
        }
    }
}
