using TechCraftsmen.User.Core.Utils;
using TechCraftsmen.User.Core.ValueObjects;
using TechCraftsmen.User.Tests.Configuration.Attributes;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Utils
{
    public class HashUtilsTests
    {
        private const string SampleText = "Text to be hashed";
        
        [UnitFact("HashUtils")]
        public void Should_HashText_And_ReturnHashAndSalt()
        {
            HashOutput hashResult = HashUtils.HashText(SampleText);

            Assert.NotNull(hashResult);
            Assert.NotEmpty(hashResult.Hash);
            Assert.NotEmpty(hashResult.Salt);
        }
        
        [UnitFact("HashUtils")]
        public void Should_KeepHashAndSalt_ForTheSameText_When_SaltIsPassed()
        {
            HashOutput hashResult = HashUtils.HashText(SampleText);
            HashOutput hashResultToCompare = HashUtils.HashText(SampleText, hashResult.Salt);

            Assert.True(hashResult.Hash.SequenceEqual(hashResultToCompare.Hash));
            Assert.True(hashResult.Salt.SequenceEqual(hashResultToCompare.Salt));
        }
        
        [UnitFact("HashUtils")]
        public void Should_ChangeHashAndSalt_ForTheSameText_When_SaltIsNotPassed()
        {
            HashOutput hashResult = HashUtils.HashText(SampleText);
            HashOutput hashResultToCompare = HashUtils.HashText(SampleText);

            Assert.False(hashResult.Hash.SequenceEqual(hashResultToCompare.Hash));
            Assert.False(hashResult.Salt.SequenceEqual(hashResultToCompare.Salt));
        }
        
        [UnitFact("HashUtils")]
        public void Should_ReturnTrue_When_VerifyingWithCorrectHashAndSalt()
        {
            HashOutput hashResult = HashUtils.HashText(SampleText);

            bool verifyResult = HashUtils.VerifyHash(SampleText, hashResult);

            Assert.True(verifyResult);
        }
        
        [UnitFact("HashUtils")]
        public void Should_ReturnFalse_When_VerifyingWithIncorrectHash()
        {
            HashOutput hashResult = HashUtils.HashText($"{SampleText}a");

            bool verifyResult = HashUtils.VerifyHash(SampleText, hashResult);

            Assert.False(verifyResult);
        }
        
        [UnitFact("HashUtils")]
        public void Should_ReturnFalse_When_VerifyingWithIncorrectSalt()
        {
            HashOutput hashResult = HashUtils.HashText($"{SampleText}");
            byte[] randomSalt = HashUtils.CreateSalt();

            hashResult.Salt = randomSalt;

            bool verifyResult = HashUtils.VerifyHash(SampleText, hashResult);

            Assert.False(verifyResult);
        }
    }
}
