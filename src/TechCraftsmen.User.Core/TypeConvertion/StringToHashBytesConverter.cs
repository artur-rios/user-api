using AutoMapper;
using TechCraftsmen.User.Core.Utils;

namespace TechCraftsmen.User.Core.TypeConvertion
{
    public class StringToHashBytesConverter : ITypeConverter<string, byte[]>
    {
        public byte[] Convert(string source, byte[] destination, ResolutionContext context)
        {
            return HashUtils.HashText(source).Hash;
        }
    }
}
