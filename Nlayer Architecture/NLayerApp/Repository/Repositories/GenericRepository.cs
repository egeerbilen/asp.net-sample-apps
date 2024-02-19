using Core.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class // T nin de bir class olduğunu belirttik yoksa ne olduğu belli değil where ile yazdığımız ifade generic tipli bir class diyoruz
    {
        // protected çünkü ilerde temel crud operasyonları dışında da bir şeye ihtiyaç duyabileceğim için
        // protected: sadece miras alınan sınıflardan erişmek için
        protected readonly AppDbContext _context;
        // dbSet bizim veri tabanındaki tablomuza karşılık geliyor
        // readonly çünkü bu değişkenler constroctur ve ya altta değer atana bilir sonrasında değer atanamaz
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity); // AddAsync metodu, Entity Framework'teki bir DbSet nesnesine yeni bir nesne eklemek için kullanılır. Bu metod, veritabanına asenkron bir şekilde bir nesne eklemeyi sağlar. Özellikle async/await yapısını kullanarak asenkron olarak çalışan programlar için faydalıdır.
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities); // AddRangeAsync metodu, Entity Framework'teki bir DbSet nesnesine toplu halde yeni nesneler eklemek için kullanılır. Bu metod, bir koleksiyon veya liste içinde bulunan birden fazla nesneyi veritabanına asenkron bir şekilde eklemeyi sağlar.
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression); // AnyAsync, Entity Framework'te sorgulama sonuçları üzerinde koşulun doğruluğunu kontrol etmek için kullanılan bir metottur. Bu metot, belirli bir sorgu sonucunda veri var mı yok mu, yani sorgu sonucunda herhangi bir kayıt dönüp dönmediğini kontrol etmek için kullanılır.
        }

        public IQueryable<T> GetAll()
        {
            // IQueryable neden olurda datayı aldık dan sonra veri tabanına daha bu gitmedi order by yapabilirim
            // AsNoTracking diyorum ki ef core çekmiş olduğu dataları memory e almasın track etmesin ki daha performanslı çalışsın
            // AsNoTracking demez sen 10000 data. Sallıyorum datayı alır ve anlık olarak bunların durmunu izler ve buda uygulamanın performansını düşürür
            return _dbSet.AsNoTracking().AsQueryable(); // AsQueryable, bir koleksiyonu veya bir IEnumerable türetilmiş nesneyi sorgulanabilir bir yapıya dönüştürmek için kullanılan bir metottur. Bu metod, LINQ sorgularını ve Entity Framework gibi sorgu tabanlı teknolojileri kullanabilmek için bir koleksiyonu veya veri setini sorgulanabilir bir formata dönüştürür.
            // Bu, sorgu sonuçlarındaki nesneler üzerinde yapılan değişikliklerin veritabanına geri yansıtılmayacağı anlamına gelir.
            // AsNoTracking, Entity Framework'te sorgu sonuçları üzerinde takip (tracking) işlemini devre dışı bırakmak için kullanılan bir metottur. Bu metot, sorgulanan verilerin değişikliklerini takip etmeyi devre dışı bırakarak bellek kullanımını azaltır ve performansı artırır.
            // ama .AsNoTracking() metotdu sorgulanan verilerin takibini bırakarak, veritabanından okunan nesnelerin DbContext'teki değişikliklerini takip etmemeyi sağlar. Bu genellikle okuma işlemleri için faydalıdır, çünkü verilerin sadece okunması ve değiştirilmemesi durumunda performans artışı sağlayabilir.
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id); // FindAsync, Entity Framework'te birincil anahtar (primary key) değerine dayalı olarak bir nesneyi asenkron bir şekilde bulmak için kullanılan bir metottur. Bu metod, bir varlık sınıfının birincil anahtar değerine göre veritabanında bir nesneyi arar ve bulursa o nesneyi getirir. Bulunan nesne yoksa null döndürür.
        }

        public void Remove(T entity)
        {
            // async si neden yok çükü db den silmez sadece o entity nin statini delete olarak işaretliyoruz flag koyuyoruz
            // savechange metodunu çağırınca ef core o deleted flagleri bulup gidip db den onları siliyor 
            //_context.Entry(entity).State = EntityState.Deleted; alt daki kod ile bu aynı şey
            // async ye gerek yok çünkü memory deki state sadece enum değer atıyoruz yüklü bir işlem değil
            _dbSet.Remove(entity); // Remove, Entity Framework'te bir varlık nesnesini veritabanından silmek için kullanılan bir metottur. Bu metod, bir varlık nesnesini veritabanından kaldırmak ve ilişkili verileri de silmek için kullanılır.

        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities); // RemoveRange, Entity Framework'te bir koleksiyon veya IEnumerable türünden birden fazla varlık nesnesini veritabanından silmek için kullanılan bir metottur. Bu metot, belirtilen varlık nesnelerinin hepsini veya belirli bir koşulu sağlayan varlık nesnelerini toplu halde silmek için kullanılır.
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity); // Update metodunun Entity Framework Core'da kullanımı, bir varlık nesnesinin veritabanındaki karşılığını güncellemek için kullanılır. Bu metot, belirtilen varlık nesnesinin durumunu "değiştirilmiş" (Modified) olarak işaretler ve sonraki SaveChanges çağrısında veritabanında güncellenmesini sağlar.
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression); // Bu Where metodunun görevi, LINQ sorgusu üzerinde filtreleme yapmak için kullanılır. Expression<Func<T, bool>> olarak ifade edilen parametre, filtreleme koşulunu tanımlayan bir lambda ifadesini alır.
                                             //  LINQ sorgusu kod örneği products.Where(p => p.Price > 20); // Fiyatı 20'den büyük ürünleri filtrele

            // doğru dan SQL sorgusu kullanmaya Native SQL denir
            // var query = "SELECT * FROM Products WHERE Price > @price"; // Örnek SQL sorgusu
            // var priceParameter = new SqlParameter("@price", 20); // Parametre oluşturulması
            // context.Products.FromSqlRaw(query, priceParameter).ToList();
        }
    }
}
