using Core.DTOs;
using Core.Model;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.Services;

namespace API.Controllers
{
    public class CategoryController : CustomBaseController
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return CreateActionResult(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoryDto category)
        {
            return CreateActionResult(await _service.AddAsync(category));
        }

        // api/categories/GetSingleCategoryByIdWithProducts/2
        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetSingleCategoryByIdWithProducts(int categoryId)
        {

            return CreateActionResult(await _service.GetSingleCategoryByIdWithProductsAsync(categoryId));

        }

        [HttpPost("/SaveAllCategory")]
        public async Task<IActionResult> Save(List<CategoryDto> categoryDto) // TODO ProductDto yo ProductCreatedDto yap 
        {
            return CreateActionResult(await _service.AddRangeAsync(categoryDto));
        }

        [HttpDelete("/DelteAllCategory")]
        public async Task<IActionResult> RemoveAll(List<int> ids)
        {
            return CreateActionResult(await _service.RemoveRangeAsync(ids));
        }
    }
}
