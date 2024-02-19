using Core.Model;
using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using Repository;
using Repository.Repositories; 

namespace NLayer.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
        // Bir metot gelir çünkü zaten GenericRepository<Product> zaten bunradaki çoğı şeyi buraya miras aldık yani
        // ProductRepository üzerinden hem GenericRepository de ki metotlara erişe bileceğim hemde burada eklenen ekstra metoda erişe bileceğim
        // sadeve IProductRepository imp etseyrin IProductRepository den ki her şeyi buraya eklemek zorundaydım bu yüzden GenericRepository yi buraya miras alıyorum
        public async Task<List<Product>> GetProductsWitCategoryAsync()
        {
            // Include dediğimde neleri de dahil etmek istiyorum onu yazdık
            // Include, Entity Framework'te kullanılan bir metottur ve sorgu sonucunda ilişkili verileri (bağlantılı tabloları) almak için kullanılır.
            // Entity Framework'te ilişkili tablolar arasında bir ilişki (relationship) tanımlandığında, varsayılan olarak sorgular yalnızca doğrudan seçilen tabloya ait verileri getirir. Ancak, genellikle ilişkili tablolardan gelen verilere de ihtiyaç duyulur. İşte burada Include devreye girer.
            // Includemetodu ile Eager loading yaptık yani dataları da çekerken katagorilerinde alınmasını istedik
            return await _context.Products.Include(x => x.Category).ToListAsync();
        }
    }
}
