using Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Stock).IsRequired();
            builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)"); // toplam 18 karakter virgülden sonra 2 karakter yani toplam 18 karakter var "," solunda 16 sağında 2 karakter var

            builder.ToTable("Products");

            // builder.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId); 
            // burada eğer Category_Id şeklinde yazsaydık ef core bunu anlayamayacaktı bu yüzden bu üst satırdaki kodu yazmamız gerekecekti

        }
    }
}
