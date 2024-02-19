using AutoMapper;
using Core.DTOs;
using Core.Model;
using Core.Repositories;
using Core.Services;
using Core.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core.DTOs;
using NLayer.Core.Repositories;
using NLayer.Repository.Repositories;
using Service.Services;

namespace NLayer.Caching
{
    public class ProductWithCachingService : GenericService<Product, ProductDto>, IProductService
    {

        // Cache de ki datamız kesinlikler çok sık erişeceğimiz ama çok sık değiştirmeyeceğimiz bir data olmalıdır muhakkak
        private const string CacheProductKey = "productsCache";
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _repository;

        public ProductWithCachingService(IProductRepository repository, IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache) : base(repository, unitOfWork, mapper)
        {
            _memoryCache = memoryCache;
            _repository = repository;
        }

        public async Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWitCategoryAsync()
        {
             
            if (_memoryCache.TryGetValue(CacheProductKey, out List<ProductWithCategoryDto> cachedProducts))
            {
                return CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, cachedProducts);
            }
            else
            {
                var products = await _repository.GetProductsWitCategoryAsync();
                var productsDto = _mapper.Map<List<ProductWithCategoryDto>>(products);

                // Cache the data for future use
                _memoryCache.Set(CacheProductKey, productsDto, TimeSpan.FromSeconds(5)); // Set cache for 10 minutes

                return CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productsDto);
            }
        }

        public Task<CustomResponseDto<ProductDto>> AddAsync(ProductCreateDto dto)
        {
            throw new NotImplementedException();
        }


        public Task<CustomResponseDto<NoContentDto>> UpdateAsync(ProductUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
