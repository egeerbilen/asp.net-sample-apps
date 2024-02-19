using RedisSample.Interfaces;
using RedisSample.Model;
using StackExchange.Redis;

namespace RedisSample.Services;
public class CategoryService : BaseCacheService, ICategoryService
{
    public readonly BaseCacheService _cacheService; // Kategori hizmeti için önbellek servisi arayüzü.

    public CategoryService(IConnectionMultiplexer redisCon) : base(redisCon)
    {
    }

    public List<CategoryModel> GetAllCategory() // Tüm kategorileri getiren metot.
    {
        List<CategoryModel> categories = new List<CategoryModel>
        {
            new CategoryModel { Id = 1, Name = "Electronic" },
            new CategoryModel { Id = 2, Name = "Clothes" }
        };

        // "allcategories" anahtarı ile önbellekteki kategorileri alma veya ekleme işlemi yapılır.
        // Şimdi de GetOrAdd fonksiyonumuzu kullanalım. Örneğin bir kategorilerimiz olsun category/getall çağırdığımız zaman önce
        // cache’den almayı deneyecek eğer yoksa sorgumuz çalışacak cache’e ekleyecek ve sorgu dönecek.
        return _cacheService.GetOrAdd("allcategories", () => { return categories; });
    }

}

