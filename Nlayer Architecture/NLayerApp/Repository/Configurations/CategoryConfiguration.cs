using Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configurations
{
    // Fluent API Kullanımı ile ekledik
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category> // dış taraftan erişmeyeceğimiz için accesmodifiers internal kalabilir
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(); // id değeri 1 er birer artar bir şey belirtmezsek
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(50);

            builder.ToTable("Categories"); // -> bunu vermezsek default olarak propert değerini alır
        }

    }
}
