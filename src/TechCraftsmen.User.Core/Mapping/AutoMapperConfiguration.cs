using AutoMapper;

namespace TechCraftsmen.User.Core.Mapping
{
    public static class AutoMapperConfiguration
    {
        public static MapperConfiguration UserMapping => new(config =>
        {
            config.AddProfile<AutoMapperProfile>();
        });
    }
}
