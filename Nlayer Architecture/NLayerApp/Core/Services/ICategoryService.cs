using Core.DTOs;
using Core.Model;
using Core.Services;
using NLayer.Core.DTOs;

namespace NLayer.Core.Services
{
    public interface ICategoryService : IGenericService<Category, CategoryDto>
    {
        public Task<CustomResponseDto<CategoryWithProductsDto>> GetSingleCategoryByIdWithProductsAsync(int categoryId);

    }
}
