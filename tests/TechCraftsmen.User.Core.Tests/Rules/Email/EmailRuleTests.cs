using TechCraftsmen.User.Core.Rules.Email;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Rules.Email
{
    public class EmailRuleTests
    {
        private readonly EmailRule _rule = new();

        [Theory]
        [Unit("EmailRule")]
        [InlineData(" ")]
        [InlineData("-------")]
        [InlineData("@majjf.com")]
        [InlineData("A@b@c@example.com")]
        [InlineData("Abc.example.com")]
        [InlineData("js@proseware..com")]
        [InlineData("ma@@jjf.com")]
        [InlineData("ma@jjf.")]
        [InlineData("ma@jjf..com")]
        [InlineData("ma@jjf.c")]
        [InlineData("ma_@jjf")]
        [InlineData("ma_@jjf.")]
        [InlineData("j@proseware.com9")]
        [InlineData("js@proseware.com9")]
        [InlineData("ma@hostname.comcom")]
        [InlineData("MA@hostname.coMCom")]
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
        public void Should_ReturnFalse_For_NullOrEmptyEmail(string email)
        {
            var result = _rule.Execute(email);

            Assert.False(result.Success);
            Assert.True(result.Errors.Any());
            Assert.Equal("Parameter null or empty", result.Errors.FirstOrDefault());
        }

        [Theory]
        [Unit("EmailRule")]
        [InlineData("ma_@jjf.com")]
        [InlineData("12@hostname.com")]
        [InlineData("d.j@server1.proseware.com")]
        [InlineData("david.jones@proseware.com")]
        [InlineData("j.s@server1.proseware.com")]
        [InlineData("jones@ms1.proseware.com")]
        [InlineData("m.a@hostname.co")]
        [InlineData("m_a1a@hostname.com")]
        [InlineData("ma.h.saraf.onemore@hostname.com.edu")]
        [InlineData("ma@hostname.com")]
        [InlineData("ma12@hostname.com")]
        [InlineData("ma-a.aa@hostname.com.edu")]
        [InlineData("ma-a@hostname.com")]
        [InlineData("ma-a@hostname.com.edu")]
        [InlineData("ma-a@1hostname.com")]
        [InlineData("ma.a@1hostname.com")]
        [InlineData("ma@1hostname.com")]
        public void Should_ReturnTrue_For_ValidEmail(string email)
        {
            var result = _rule.Execute(email);

            Assert.True(result.Success);
            Assert.False(result.Errors.Any());
        }
    }
}
