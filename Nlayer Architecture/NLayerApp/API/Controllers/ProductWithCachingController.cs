using AutoMapper;
using Core.DTOs;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.Services;
using Service.Services;

namespace API.Controllers
{
    public class ProductWithCachingController : CustomBaseController
    {
        private readonly IProductService _service; 

        public ProductWithCachingController(IProductService productWithCachingService)
        {
            _service = productWithCachingService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductsWithCategory()
        {
            return CreateActionResult(await _service.GetProductsWitCategoryAsync());
        }
    }
}
