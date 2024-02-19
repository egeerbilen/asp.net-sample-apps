using Core.Model;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Category> GetSingleCategoryByIdWithProductsAsync(int categoryId)
        {
            // context üzerinden Categories lere git ve Categories bağlı olduğu productları da dahilet
            // FirstOrDefaultAsync kullanmak yerine SingleOrDefaultAsync kullanıyoruz çünkü x => x.Id == categoryId koşulunu sağlayan birden çok sağlarsa hata döner
            // FirstOrDefaultAsync da ilk elemanı döndürür zaten birden fazla varsa ciddi bir sıkıntı var demekdir
            // return await _context.Categories.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == categoryId);
            // Kısaca x => x.Id == categoryId koşulunu sağlayan birden fazla öğe varsa SingleOrDefaultAsync kullanmanın bir istisna fırlatacağıdır.
            // Çünkü benzersiz bir kimlik için yalnızca bir veya hiç eşleşme olmalıdır. Öte yandan FirstOrDefaultAsync,
            // ilk eşleşen öğeyi döndürür ve bu durumda çoklu eşleşmeler varsa altta yatan sorunu gizleyebilir.

            return await _context.Categories.Include(x => x.Products).Where(x => x.Id == categoryId).SingleOrDefaultAsync();
        }
    }
}
