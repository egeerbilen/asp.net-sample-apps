using AutoMapper;
using AutomapperWebAPI.Entities;
using AutomapperWebAPI.DTO;
//using AutomapperWebAPI.HelperFunctions;
namespace AutomapperWebAPI.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapleme işleminin yapıldığı yer burası maplerken full name içine FirstName ve LastName değerlerini birleştirerek yaz diyoruz
            CreateMap<User, UserReadDto>().ForMember(dest => dest.FullName, opt => opt.MapFrom(nms => $"{nms.FirstName} ---- {nms.LastName}"));

            CreateMap<UserCreateDto, User>();
        }
    }
}
