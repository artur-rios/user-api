using System.Text;
using TechCraftsmen.User.Core.Utils;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Utils
{
    public class HashUtilsTests
    {
        private const string SAMPLE_TEXT = "Text to be hashed";

        [Fact]
        public void Should_HashText_And_ReturnHashAndSalt()
        {
            var hashResult = HashUtils.HashText(SAMPLE_TEXT);

            Assert.NotNull(hashResult);
            Assert.NotEmpty(hashResult.Hash);
            Assert.NotEmpty(hashResult.Salt);
        }

        [Fact]
        public void Should_KeepHashAndSalt_ForTheSametext_When_SaltIsPassed()
        {
            var hashResult = HashUtils.HashText(SAMPLE_TEXT);
            var hashResultToCompare = HashUtils.HashText(SAMPLE_TEXT, hashResult.Salt);

            Assert.True(hashResult.Hash.SequenceEqual(hashResultToCompare.Hash));
            Assert.True(hashResult.Salt.SequenceEqual(hashResultToCompare.Salt));
        }

        [Fact]
        public void Should_ChangeHashAndSalt_ForTheSameText_When_SaltIsNotPassed()
        {
            var hashResult = HashUtils.HashText(SAMPLE_TEXT);
            var hashResultToCompare = HashUtils.HashText(SAMPLE_TEXT);

            Assert.False(hashResult.Hash.SequenceEqual(hashResultToCompare.Hash));
            Assert.False(hashResult.Salt.SequenceEqual(hashResultToCompare.Salt));
        }

        [Fact]
        public void Should_ReturnTrue_When_VerifyingWithCorrectHashAndSalt()
        {
            var hashResult = HashUtils.HashText(SAMPLE_TEXT);

            string hashString = Encoding.UTF8.GetString(hashResult.Hash, 0, hashResult.Hash.Length);
            string saltString = Encoding.UTF8.GetString(hashResult.Salt, 0, hashResult.Salt.Length);

            var verifyResult = HashUtils.VerifyHash(SAMPLE_TEXT, hashString, saltString);

            Assert.True(verifyResult);
        }

        [Fact]
        public void Should_ReturnFalse_When_VerifyingWithIncorrectHash()
        {
            var hashResult = HashUtils.HashText($"{SAMPLE_TEXT}a");

            string hashString = Encoding.UTF8.GetString(hashResult.Hash, 0, hashResult.Hash.Length);
            string saltString = Encoding.UTF8.GetString(hashResult.Salt, 0, hashResult.Salt.Length);

            var verifyResult = HashUtils.VerifyHash(SAMPLE_TEXT, hashString, saltString);

            Assert.False(verifyResult);
        }

        [Fact]
        public void Should_ReturnFalse_When_VerifyingWithIncorrectSalt()
        {
            var hashResult = HashUtils.HashText($"{SAMPLE_TEXT}");
            var randomSalt = Encoding.UTF8.GetString(HashUtils.CreateSalt());

            string hashString = Encoding.UTF8.GetString(hashResult.Hash, 0, hashResult.Hash.Length);

            var verifyResult = HashUtils.VerifyHash(SAMPLE_TEXT, hashString, randomSalt);

            Assert.False(verifyResult);
        }
    }
}
