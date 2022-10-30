using AutoMapper;
using Models;
using ModelsDb;

namespace Services
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Ad, AdDb>().ReverseMap();

            CreateMap<User, UserDb>().ReverseMap();
        }
    }
}
