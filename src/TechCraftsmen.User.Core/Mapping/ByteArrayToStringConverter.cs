using AutoMapper;
using System.Text;

namespace TechCraftsmen.User.Core.Mapping
{
    public class ByteArrayToStringConverter : ITypeConverter<byte[], string>
    {
        public string Convert(byte[] source, string destination, ResolutionContext context)
        {
            return Encoding.UTF8.GetString(source, 0, source.Length);
        }
    }
}
