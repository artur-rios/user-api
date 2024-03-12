using AutoMapper;

namespace TechCraftsmen.User.Core.Utils
{
    public class StringToHashBytesConverter : ITypeConverter<string, byte[]>
    {
        public byte[] Convert(string source, byte[] destination, ResolutionContext context)
        {
            return HashUtils.HashText(source).Hash;
        }
    }
}
