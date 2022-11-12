using AutoMapper;
using Models;
using ModelsDb;

namespace Services
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Announcement, AnnouncementDb>().ReverseMap();

            CreateMap<User, UserDb>().ReverseMap();
        }
    }
}
