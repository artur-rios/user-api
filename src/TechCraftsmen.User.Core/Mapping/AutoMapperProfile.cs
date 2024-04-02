using AutoMapper;
using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Core.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<byte[], string>().ConvertUsing<ByteArrayToStringConverter>();
            CreateMap<string, byte[]>().ConvertUsing<StringToHashBytesConverter>();

            CreateMap<HashDto, PasswordDto>().ForMember(passwordDto => passwordDto.Password, option => option.MapFrom(src => src.Hash));
            CreateMap<PasswordDto, HashDto>().ForMember(hashDto => hashDto.Hash, option => option.MapFrom(src => src.Password));

            CreateMap<Entities.User, UserDto>().ForMember(user => user.Password, option => option.Ignore());
            CreateMap<UserDto, Entities.User>().ForMember(user => user.Password, option => option.Ignore());
        }

        private void CreateTwoWayMap<T1, T2>()
        {
            CreateMap<T1, T2>();
            CreateMap<T2, T1>();
        }
    }
}
