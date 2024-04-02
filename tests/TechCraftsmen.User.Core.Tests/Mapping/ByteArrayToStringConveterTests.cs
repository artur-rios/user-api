using System.Text;
using Xunit;

namespace TechCraftsmen.User.Core.Tests.Mapping
{
    public class ByteArrayToStringConveterTests
    {
        private const string SAMPLE_TEXT = "Text to be converted";

        [Fact]
        public void ShouldConvertByteArrayToString()
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(SAMPLE_TEXT);

            string result = Encoding.UTF8.GetString(textBytes, 0, textBytes.Length);

            Assert.Equal(SAMPLE_TEXT, result);
        }
    }
}
