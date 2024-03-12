using AutoMapper;
using TechCraftsmen.User.Core.Dto;

namespace TechCraftsmen.User.Core.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<string, byte[]>().ConvertUsing<StringToHashBytesConverter>();

            CreateMap<Core.Entities.User, UserDto>().ForMember(user => user.Password, option => option.Ignore());
            CreateMap<UserDto, Core.Entities.User>().ForMember(user => user.Password, option => option.Ignore());
        }

        private void CreateTwoWayMap<T1, T2>()
        {
            CreateMap<T1, T2>();
            CreateMap<T2, T1>();
        }
    }
}
