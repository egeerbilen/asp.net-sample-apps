using System.Linq.Expressions;

namespace Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        // yazılımda olduğunca soyut neslener ie çalışmak önemli
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetAll();
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        // IEnumerable tipi veriyi önce belleğe atıp ardından bellekteki bu veri üzerinden belirtilen koşulları çalıştırır ve veriye uygular.
        // IQueryable tipinde ise belirtilen sorgular direk olarak server üzerinde çalıştırılır ve dönüş sağlar.Ayrıca bu tip IEnumerable tipini implement ettiği için
        // IEnumerable’ın tüm özelliklerini kullanabilir.

        // Performans:
        // IEnumerable: Veriler yerel olarak değerlendirildiği için, tüm veriler hafıza üzerinde işlenir.Bu, büyük veri setlerinde performans sorunlarına yol açabilir.
        // IQueryable: Sorgu veritabanına taşınabilir, böylece sadece gerekli veri alınır.Bu, performans açısından daha etkili olabilir, özellikle büyük veri setlerinde.

        // Eğer GetAll metodu, bir veritabanı sorgusunu temsil ediyorsa ve sorgunun veritabanında çalıştırılmasını istiyorsanız, IQueryable<T> kullanmak genellikle daha uygun olacaktır. IQueryable sorguları, genellikle veritabanına daha etkili bir şekilde çevrilebilir ve optimize edilebilir.
        // Ancak, veri kümesi küçükse ve sorgular genellikle bellek üzerinde işleniyorsa veya GetAll metodu bir koleksiyonu döndürüyorsa, IEnumerable<T> kullanmak daha uygundur.


        // where metodundan geriye bir IQuery metodu dönüyorum ki ben döndükden sonra başka sorgular yazabileyim. Özetle veri tabanına sorgu yapmıyoruz veri tabanına yapılacak olan sorguyu burada oluşturuyoruz
        // Tipini list yaparsak gider db den datayı direk olarak çeker datayı çektikden sonra memory e alır memory e aldıkdan sonra order by yapar
        // IQueryable wher den sonra order byı da alır ve sıralar
        // boolean dönüş tipine denkgeliyor eşleşenleri istedğimizde true değerlerini döndürecek

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity); // update ve remove dediğimizde sadece o memory de takip etmiş olduğumuz stati ni modify ettiği için uzun sürmüyor
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
