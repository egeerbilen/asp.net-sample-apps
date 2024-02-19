using AutoMapper;
using Core.DTOs;
using Core.Model;
using NLayer.Core.DTOs;

namespace Service.Mapping
{
    // Entity leri DTO ya ya da DTO ları Entity e çevirecek
    // Mapleme işlemini burada belirteceğiz
    public class MapProfile : Profile
    {
        // tüm hepsini buraya yazmaya gerek yok parçalı bir şekilde de yazılabilir bu fazla olduğu zamanda
        public MapProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap(); // ReverseMap diyerek ProductDto yu Product ya da çevire bilme özelliğini keledik
            // yani hem Product -> ProductDto ya hem de ProductDto -> Product çevire biliyoruz
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<ProductFeature, ProductFeatureDto>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>(); // Reverse gerekyok çünkü her hangi bir update de  Product dan ProductUpdateDto ya dönüştürmeme gerekyok bu senaryoda ondan dolayı ReverseMap yazmana gerek yok
            CreateMap<Product, ProductWithCategoryDto>();
            CreateMap<Category, CategoryWithProductsDto>();
            CreateMap<ProductCreateDto, Product>();
        }
    }
}
