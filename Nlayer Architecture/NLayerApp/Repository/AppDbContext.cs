using Core.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Repository
{
    public class AppDbContext : DbContext
    {
        // DbContextOptions ile startup dosyasından bunu kolayca vereceğiz bu da AppDbContext için options olacak
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // ProductFeature özelliklerini buradan da vere bilirim 
            // yeni bir ProductFeature eklemek istediğimizde mutlaka var olan Productun bağımlı bir şekilde eklenir bu gerçek dünyada kullanılır
            // ProductFeature bir Product üzerinden işlem görmesi çok daha bestPractice olacaktır
            // var p = new Product() { ProductFeature = new ProductFeature() { } };
        }

        public DbSet<Category> Categories { get; set; }
        // Product nesnesi üzerinden eklemesi için
        public DbSet<Product> Products { get; set; }
        //public DbSet<ProductFeature> ProductFeatures { get; set; } // ProductFeatures Bu şekilde eklersek bağımsız olarak ProductFeatures satırlarını bağımsız olarak ekleye bilirim veya güncelleye bilirim

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Model oluşurken çalışacak olan method

            // Bu kod parçası, Entity Framework Core'da veritabanı tablolarını modele eşlemek için kullanılan bir ModelBuilder yöntemidir. Bu özel durumda, Category adlı bir sınıfın Id özelliğini birincil anahtar olarak belirler.
            // modelBuilder.Entity<Category>().HasKey(x => x.Id); 
            // burayı kirletmemek için her bir entity ile ilgili ayarı farkıl ibr class da yaparız

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // ApplyConfigurationsFromAssembly metodu bu assembly içindeki tüm Entity Framework yapılandırma sınıflarını bulur ve bunları veritabanı modelinizi oluştururken kullanır.
            // Assembly.GetExecutingAssembly() yöntemi, mevcut uygulama içindeki yürütülebilir dosyanın derlenmiş kodlarını temsil eden Assembly nesnesini döndürür.

            // modelBuilder.ApplyConfiguration(new ProductConfiguration()); -> bu şekilde tek tek de verile bilir ama 100 lerce olduğunu düşünürsek fazla olur


            // alt taraftta ki eklenen da ta ProductFeature seeds altında gelmeliydi ama örnek olarak ekliyoruz şimdilik best practice uymuyor
            modelBuilder.Entity<ProductFeature>().HasData(new ProductFeature()
            {
                Id = 1,
                Color = "Kırmızı",
                Height = 100,
                Width = 200,
                ProductId = 1
            },
            new ProductFeature()
            {
                Id = 2,
                Color = "Mavi",
                Height = 300,
                Width = 500,
                ProductId = 2
            });
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            UpdateChangeTracker();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateChangeTracker();
            return base.SaveChangesAsync(cancellationToken);
        }

        public void UpdateChangeTracker()
        {
            foreach (var item in ChangeTracker.Entries()) // ChangeTracker sınıfı, DbContext içindeki varlık nesnelerinin durumlarını ve değişikliklerini izleme yeteneği sağlar. Entries() metodu, değişiklik izleyicisindeki tüm varlık girişlerini bir koleksiyon olarak döndürür.
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;

                                entityReference.UpdatedDate = DateTime.Now;
                                break;
                            }


                    }
                }
            }
        }
    }
}
