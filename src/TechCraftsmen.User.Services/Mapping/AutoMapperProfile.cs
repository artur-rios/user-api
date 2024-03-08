using AutoMapper;
using TechCraftsmen.User.Common.Dto;
using TechCraftsmen.User.Common.TypeConvertion;

namespace TechCraftsmen.User.Services.Mapping
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
