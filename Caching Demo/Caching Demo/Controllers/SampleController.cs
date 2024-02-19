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
            // Verinin �nbellekte olup olmad���n� kontrol et
            if (_memoryCache.TryGetValue("CachedData", out string cachedData))
            {
                return Ok($"Cached Data: {cachedData}");
            }

            // E�er �nbellekte yoksa, veriyi kaynaktan al
            string newData = FetchDataFromSource();

            // Veriyi �nbelle�e 5 saniye bir s�reyle sakla
            _memoryCache.Set("CachedData", newData, TimeSpan.FromSeconds(5));

            return Ok($"Fresh Data: {newData}");
        }

        private string FetchDataFromSource()
        {
            // Ger�ek bir senaryoda, veriyi veri kayna��n�zdan alman�z gerekir
            // Bu sadece bir yer tutucu metodudur
            return "Data from source";
        }
    }
}
