using RedisSample.Model;

namespace RedisSample.Interfaces;

public interface ICategoryService : IBaseCacheService
{
    // Tüm kategorileri getiren metot.
    List<CategoryModel> GetAllCategory();
}

