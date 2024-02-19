using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Caching_Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        public SampleController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet("cached-data")]
        public IActionResult GetCachedData()
        {
            // Verinin önbellekte olup olmadýðýný kontrol et
            if (_memoryCache.TryGetValue("CachedData", out string cachedData))
            {
                return Ok($"Cached Data: {cachedData}");
            }

            // Eðer önbellekte yoksa, veriyi kaynaktan al
            string newData = FetchDataFromSource();

            // Veriyi önbelleðe 5 saniye bir süreyle sakla
            _memoryCache.Set("CachedData", newData, TimeSpan.FromSeconds(5));

            return Ok($"Fresh Data: {newData}");
        }

        private string FetchDataFromSource()
        {
            // Gerçek bir senaryoda, veriyi veri kaynaðýnýzdan almanýz gerekir
            // Bu sadece bir yer tutucu metodudur
            return "Data from source";
        }
    }
}
