using TechCraftsmen.User.Tests.Configuration.Attributes;
using TechCraftsmen.User.Tests.Mock.Generators;
using TechCraftsmen.User.Utils.Security;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Utils
{
    public class HashUtilsTests
    {
        private const string SampleText = "Text to be hashed";
        
        [UnitFact("HashUtils")]
        public void Should_HashText_And_ReturnHashAndSalt()
        {
            Hash hash = new(SampleText);

            Assert.NotNull(hash);
            Assert.NotEmpty(hash.Value);
            Assert.NotEmpty(hash.Salt);
        }
        
        [UnitFact("HashUtils")]
        public void Should_KeepHashAndSalt_ForTheSameText_When_SaltIsPassed()
        {
            Hash hash = new(SampleText);
            Hash hashResultToCompare = new(SampleText, hash.Salt);

            Assert.True(hash.Value.SequenceEqual(hashResultToCompare.Value));
            Assert.True(hash.Salt.SequenceEqual(hashResultToCompare.Salt));
        }
        
        [UnitFact("HashUtils")]
        public void Should_ChangeHashAndSalt_ForTheSameText_When_SaltIsNotPassed()
        {
            Hash hash = new(SampleText);
            Hash hashResultToCompare = new(SampleText);

            Assert.False(hash.Value.SequenceEqual(hashResultToCompare.Value));
            Assert.False(hash.Salt.SequenceEqual(hashResultToCompare.Salt));
        }
        
        [UnitFact("HashUtils")]
        public void Should_ReturnTrue_When_VerifyingWithCorrectHashAndSalt()
        {
            Hash hash = new(SampleText);

            bool verifyResult = hash.TextMatches(SampleText);

            Assert.True(verifyResult);
        }
        
        [UnitFact("HashUtils")]
        public void Should_ReturnFalse_When_VerifyingWithIncorrectHash()
        {
            Hash hash = new($"{SampleText}a");

            bool verifyResult = hash.TextMatches(SampleText);

            Assert.False(verifyResult);
        }
        
        [UnitFact("HashUtils")]
        public void Should_ReturnFalse_When_VerifyingWithIncorrectSalt()
        {
            byte[] randomSalt = CustomRandomNumberGenerator.RandomNumberBytes();
            
            Hash hash = new($"{SampleText}");

            hash.Salt = randomSalt;

            bool verifyResult = hash.TextMatches(SampleText);

            Assert.False(verifyResult);
        }
    }
}
