using Core.DTOs;
using Core.Model;
using NLayer.Core.DTOs;

namespace Core.Services
{
    public interface IProductCachingService : IGenericService<Product, ProductDto>
    {
        Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWitCategoryAsync();

        // Motodları overload ettik aşağıda
        // UpdateAsync için üst sınıfadan gelen bir upda te motodu var istediğimiz zaman ProductUpdateDto ya geçer bilirim yada productdto ya geçe biliriz
        Task<CustomResponseDto<NoContentDto>> UpdateAsync(ProductUpdateDto dto);
        Task<CustomResponseDto<ProductDto>> AddAsync(ProductCreateDto dto); // ProductDto dönecek -- ProductCreateDto alacak

    }
}
