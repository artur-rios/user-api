using AutoMapper;
using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Core.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<string, byte[]>().ConvertUsing<StringToHashBytesConverter>();

            CreateMap<Entities.User, UserDto>().ForMember(user => user.Password, option => option.Ignore());
            CreateMap<UserDto, Entities.User>().ForMember(user => user.Password, option => option.Ignore());
        }
    }
}
