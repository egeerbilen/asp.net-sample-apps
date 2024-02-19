using Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Seeds
{
    internal class CategorySeed : IEntityTypeConfiguration<Category> // aynı proje içindeysek ve DbContext sınıfını aynı namespace içinde tanımladıysanız, otomatik olarak referans alınacaktır. 
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Default değer atamalar burada yapılır id yerine Guidid de olanilir
            builder.HasData(
                // Id sini açık açık vermemiz önemli sadece seed data da verilir 
                new Category { Id = 1, Name = "Kalemler" },
                new Category { Id = 2, Name = "Kitaplar" },
                new Category { Id = 3, Name = "Defterler" });
        }
    }
}
