using AutoMapper;
using ef_demo.ApiModels;
using ef_demo.Core.Entities;

namespace ef_demo.MapperProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Blog, BlogDto>().ReverseMap();
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<Blog, PostBlogDto>().ReverseMap();
        }
    }
}
