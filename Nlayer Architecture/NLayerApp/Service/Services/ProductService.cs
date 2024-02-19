using AutoMapper;
using Core.DTOs;
using Core.Model;
using Core.Repositories;
using Core.Services;
using Core.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using NLayer.Core.DTOs;
using NLayer.Core.Repositories;

namespace Service.Services
{
    public class ProductService : GenericService<Product, ProductDto>, IProductService
    {
        // Direk olarak ProductRepository e erişmek için aşağodaki kodu yazdık
        private readonly IProductRepository _productRepository;

        public ProductService(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository) : base(repository, unitOfWork, mapper)
        {
            _productRepository = productRepository;
        }

        public async Task<CustomResponseDto<ProductDto>> AddAsync(ProductCreateDto dto)
        {
            var newEntity = _mapper.Map<Product>(dto);
            await _productRepository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();
            var newDto = _mapper.Map<ProductDto>(newEntity);
            return CustomResponseDto<ProductDto>.Success(StatusCodes.Status200OK, newDto);
        }

        public async Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWitCategoryAsync()
        {
            // bussine kodunu burada yazıyoruz bir try cache kullanırsak burada yazacağız
            var products = await _productRepository.GetProductsWitCategoryAsync();

            var productsDto = _mapper.Map<List<ProductWithCategoryDto>>(products); // products değerini -> List<ProductWithCategoryDto> değerine dönüştürüyoruz
            return CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productsDto);
        }

        public async Task<CustomResponseDto<NoContentDto>> UpdateAsync(ProductUpdateDto dto)
        {
            var entity = _mapper.Map<Product>(dto);
            _productRepository.Update(entity);
            await _unitOfWork.CommitAsync();
            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }
    }
}
