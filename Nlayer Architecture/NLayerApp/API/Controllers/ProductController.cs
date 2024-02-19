using Autofac.Core;
using Core.DTOs;
using Core.Model;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using Service.Services;

namespace API.Controllers
{
    public class ProductController : CustomBaseController
    {
        private readonly IProductService _productServiceWithDto;

        public ProductController(IProductService productServiceWithDto)
        {
            _productServiceWithDto = productServiceWithDto;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            return CreateActionResult(await _productServiceWithDto.GetAllAsync());

        }

        // Metot isimleri önemli değil ama metotların tipi önemli çünkü tipe göre eşleşme var
        /// GET api/products/GetProductsWithCategory
        //[HttpGet("GetProductsWithCategory")] -> bu şekilde isim belirtile bilir ama bunun yerine aşağıda ki gibi de yapıla bilir
        [HttpGet("[action]")] // action bu aşağıda belirtilen metodun direk olarak adını alacaktır
        public async Task<IActionResult> GetProductsWithCategory()
        {
            return CreateActionResult(await _productServiceWithDto.GetProductsWitCategoryAsync());
        }

        // GET /api/products/5
        // [HttpGet("{id}")], id adında bir route parametresi alır. Bu, bir HTTP isteği yapıldığında URL'de belirtilen id değerini kullanarak
        // belirli bir kaynağa erişmek için kullanılır. Örneğin, /api/example/1 gibi bir istek yapıldığında, id değeri 1 olan kaynağa erişmeyi sağlar.
        // Öte yandan[HttpGet("id")], sabit bir URL rotası belirtir.Burada id bir parametre olarak işlenmez, sadece isteğin yapıldığı URL'nin son bölümünde
        // "id" adında bir parça bekler. Yani /api/example/id gibi bir istek yapıldığında, gerçekten "id" adında bir kaynağa erişmeye çalışır.
        //
        // [ValidateFilter] -> şeklinde burada kullanamam çünkü NotFoundFilter ımız sadece IAsyncActionFilter ımızı imp ediyor. controctor da parametra aldığımızdan dolayı
        // bir filter constroctor içinde parametre alıyorsa mutlaka service filter üzerinden kullanmamız gerekir
        [ServiceFilter(typeof(NotFoundFilter<Product>))] // şeklinde kullanmamız gerekir -> not Tüm Id lerde kullanmak istersek bunu başa almamız yeterli olacaktır public class üstüne yazılır
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // debug atarsan id değeri yoksa buraya düşmeden hatanın geldiğini göre biliriz

            // yazdığımız filter daha buraya gelmeden bu id ye sahip entity var mı yok mu kontrol edecek bunun için OnActionExecutionAsync da işlem yapacağız

            return CreateActionResult(await _productServiceWithDto.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductCreateDto productDto)
        {
            return CreateActionResult(await _productServiceWithDto.AddAsync(productDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto productDto)
        {
            return CreateActionResult(await _productServiceWithDto.UpdateAsync(productDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            return CreateActionResult(await _productServiceWithDto.RemoveAsync(id));
        }

        [HttpPost("/SaveAll")]
        public async Task<IActionResult> Save(List<ProductDto> productsDtos) // TODO ProductDto yo ProductCreatedDto yap 
        {
            return CreateActionResult(await _productServiceWithDto.AddRangeAsync(productsDtos));
        }

        [HttpDelete("/DelteAll")]
        public async Task<IActionResult> RemoveAll(List<int> ids) // List kullandık çünkü dinamik bir arraye eleman ekleme çıkarma olacak controller da başka bir işlem olmayacak çünkü
        {
            return CreateActionResult(await _productServiceWithDto.RemoveRangeAsync(ids));
        }

        [HttpPost("Any/{id}")]
        public async Task<IActionResult> Any(int id)
        {
            return CreateActionResult(await _productServiceWithDto.AnyAsync(x => x.Id == id));
        }
    }
}
