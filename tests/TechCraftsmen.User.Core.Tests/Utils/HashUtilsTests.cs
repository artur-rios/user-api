﻿using TechCraftsmen.User.Core.Dto;
using TechCraftsmen.User.Core.Utils;
using TechCraftsmen.User.Tests.Utils.Traits;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Utils
{
    public class HashUtilsTests
    {
        private const string SAMPLE_TEXT = "Text to be hashed";

        [Fact]
        [Unit("HashUtils")]
        public void Should_HashText_And_ReturnHashAndSalt()
        {
            HashDto hashResult = HashUtils.HashText(SAMPLE_TEXT);

            Assert.NotNull(hashResult);
            Assert.NotEmpty(hashResult.Hash);
            Assert.NotEmpty(hashResult.Salt);
        }

        [Fact]
        [Unit("HashUtils")]
        public void Should_KeepHashAndSalt_ForTheSameText_When_SaltIsPassed()
        {
            HashDto hashResult = HashUtils.HashText(SAMPLE_TEXT);
            HashDto hashResultToCompare = HashUtils.HashText(SAMPLE_TEXT, hashResult.Salt);

            Assert.True(hashResult.Hash.SequenceEqual(hashResultToCompare.Hash));
            Assert.True(hashResult.Salt.SequenceEqual(hashResultToCompare.Salt));
        }

        [Fact]
        [Unit("HashUtils")]
        public void Should_ChangeHashAndSalt_ForTheSameText_When_SaltIsNotPassed()
        {
            HashDto hashResult = HashUtils.HashText(SAMPLE_TEXT);
            HashDto hashResultToCompare = HashUtils.HashText(SAMPLE_TEXT);

            Assert.False(hashResult.Hash.SequenceEqual(hashResultToCompare.Hash));
            Assert.False(hashResult.Salt.SequenceEqual(hashResultToCompare.Salt));
        }

        [Fact]
        [Unit("HashUtils")]
        public void Should_ReturnTrue_When_VerifyingWithCorrectHashAndSalt()
        {
            HashDto hashResult = HashUtils.HashText(SAMPLE_TEXT);

            bool verifyResult = HashUtils.VerifyHash(SAMPLE_TEXT, hashResult);

            Assert.True(verifyResult);
        }

        [Fact]
        [Unit("HashUtils")]
        public void Should_ReturnFalse_When_VerifyingWithIncorrectHash()
        {
            HashDto hashResult = HashUtils.HashText($"{SAMPLE_TEXT}a");

            bool verifyResult = HashUtils.VerifyHash(SAMPLE_TEXT, hashResult);

            Assert.False(verifyResult);
        }

        [Fact]
        [Unit("HashUtils")]
        public void Should_ReturnFalse_When_VerifyingWithIncorrectSalt()
        {
            HashDto hashResult = HashUtils.HashText($"{SAMPLE_TEXT}");
            byte[] randomSalt = HashUtils.CreateSalt();

            hashResult.Salt = randomSalt;

            bool verifyResult = HashUtils.VerifyHash(SAMPLE_TEXT, hashResult);

            Assert.False(verifyResult);
        }
    }
}
