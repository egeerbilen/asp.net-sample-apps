namespace Core.Model
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; } // Bu navigation property 

        // ICollection çünkü Product nesnesinde arama sıralama filtreleme ekleme çıkarma gibi işlemler yapmak istiyorum
        // ICollection bir C# arabirimi (interface) ve genellikle bir koleksiyonun temel özelliklerini tanımlar. Bu arabirim, koleksiyonların bir dizi işlemi desteklemesini sağlar.
        // ICollection genellikle koleksiyonların temel işlevlerini tanımlar ve koleksiyon üzerinde bir dizi işlem yapılmasına izin verir.Örneğin, öğe ekleme, kaldırma, temizleme gibi işlemleri içerir.
        // ICollection, IEnumerable arabiriminden türetilmiştir ve genellikle koleksiyonların üzerinde döngüler(iteration) yapılmasını sağlar.Ancak ICollection, IEnumerable'den daha fazla özellik ve işlevsellik içerir. Örneğin, koleksiyon üzerinde değişiklik yapılmasına izin veren Add, Remove, Clear gibi metodları içerir.
    }
}
