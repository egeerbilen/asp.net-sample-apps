using AutoMapper;
using Core.DTOs;
using Core.Model;
using Core.Repositories;
using Core.UnitOfWorks;
using NLayer.Core.DTOs;
using NLayer.Core.Services;
using Service.Services;

namespace NLayer.Service.Services
{
    public class CategoryService : GenericService<Category, CategoryDto>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IGenericRepository<Category> repository, IUnitOfWork unitOfWork, IMapper mapper, ICategoryRepository categoryRepository) : base(repository, unitOfWork, mapper)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CustomResponseDto<CategoryWithProductsDto>> GetSingleCategoryByIdWithProductsAsync(int categoryId)
        {
            var category = await _categoryRepository.GetSingleCategoryByIdWithProductsAsync(categoryId);

            var categoryDto = _mapper.Map<CategoryWithProductsDto>(category); // category -> CategoryWithProductsDto ye çevir

            return CustomResponseDto<CategoryWithProductsDto>.Success(200, categoryDto);
        }
    }
}
