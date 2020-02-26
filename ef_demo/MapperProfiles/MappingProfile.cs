using AutoMapper;
using ef_demo.ApiModels;
using ef_demo.Infrastructure.Core;

namespace ef_demo.MapperProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Blog, BlogDTO>().ReverseMap();
            CreateMap<Post, PostDTO>().ReverseMap();
        }
    }
}
