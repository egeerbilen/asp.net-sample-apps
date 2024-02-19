using Core.Model;
using Core.Repositories;

namespace NLayer.Core.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetProductsWitCategoryAsync(); // Task ile async bir işlem olacakğını belirttik
    }
}
