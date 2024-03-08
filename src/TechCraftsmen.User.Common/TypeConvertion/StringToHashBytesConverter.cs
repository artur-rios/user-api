using AutoMapper;
using TechCraftsmen.User.Common.Utils;

namespace TechCraftsmen.User.Common.TypeConvertion
{
    public class StringToHashBytesConverter : ITypeConverter<string, byte[]>
    {
        public byte[] Convert(string source, byte[] destination, ResolutionContext context)
        {
            return HashUtils.HashText(source).Hash;
        }
    }
}
