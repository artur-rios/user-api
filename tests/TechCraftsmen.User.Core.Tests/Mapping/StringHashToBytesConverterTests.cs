using System.Text;
using TechCraftsmen.User.Core.Utils;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Mapping
{
    public class StringHashToBytesConverterTests
    {
        private const string SAMPLE_TEXT = "Text to be converted";

        [Fact]
        public void ShouldConvertStringHashToBytes()
        {
            var hashResult = HashUtils.HashText(SAMPLE_TEXT);
            string hashString = Encoding.UTF8.GetString(hashResult.Hash, 0, hashResult.Hash.Length);
            string saltString = Encoding.UTF8.GetString(hashResult.Salt, 0, hashResult.Salt.Length);

            Assert.True(HashUtils.VerifyHash(SAMPLE_TEXT, hashString, saltString));
        }
    }
}
