using Core.DTOs;
using Core.Model;
using System.Linq.Expressions;

namespace Core.Services
{
    public interface IGenericService<Entity, Dto> where Entity : BaseEntity where Dto : class
    {
        // dönüş tipleri IGenericRepository ye göre farklı 
        Task<CustomResponseDto<Dto>> GetByIdAsync(int id);
        Task<CustomResponseDto<IEnumerable<Dto>>> GetAllAsync(); //Farklılık olsun diye bunu boyle yaptık -> GetAllAsync() yerine Where() de kullanıla bilir ama daha şık olması açısından GetAllAsync() kullanıyoruz

        // IQueryable<T> GetAll(Expression<Func<T, bool>> expression); productRepository.where(x=>x.id>5) butaya kadar yaptığımızyerden (.where(x=>x.id>5)) IQueryable daha veri tabanuna sorgu yapmadık farklı işlemler yapa biliriz where den sora ne zaman tosyncasing dersek o zaman sorgu yapacak
        // o yüzden bir expression bir ifade tanımlıyoruz
        // T alması x e karşılık geliyor - bolean ise dönüş tipine karşılık geliyor
        // IQueryable ile datayı aldıkdan sonra işlem yamak istersek order by gibi IQueryable kullan. IQueryable dönen şeylerde veri tabanına sorgu atılmaz bunlar memory de tutulur sirleştirilir sonra tolist ve ya tolistasync dediğimizde veri tabanına gönderilir
        Task<CustomResponseDto<IEnumerable<Dto>>> Where(Expression<Func<Entity, bool>> expression);// async değil çünkü veri tabanına bir sorgu yapmıyorum sadece veri tabanına yapılacak olan sorguyu oluşturuyorum.
                                                                                                   // Özellikle LINQ sorgularında, Expression yapısı, sorgunun SQL veya diğer sorgu dillerine çevrilmesini sağlar.
                                                                                                   // Bu sayede sorgu ifadeleri çalışma zamanında yürütülebilir ve çeşitli optimizasyonlar yapılabilir.
        Task<CustomResponseDto<bool>> AnyAsync(Expression<Func<Entity, bool>> expression);// productRepository.where(x=>x.id>5) bu şekilde bir sorgu yazdığımızda dönüş tipi bool olacak ama async olarak dönmesini istediğimiz için AnyAsync isminni verdik
        Task<CustomResponseDto<Dto>> AddAsync(Dto dto);
        Task<CustomResponseDto<IEnumerable<Dto>>> AddRangeAsync(IEnumerable<Dto> dtos);// birden fazla kaydede bilirim aldığımız nokta -> IEnumerable alıyoruz List almıyoruz çünkü mümkün olduğunca soyut nesneler ile çalışmak önemli interface veya abstract gibi
        // new anahtar sözcüğü kullanarak nesne örneği alamayız. soyutnesneleri istediğim bir tipe dönüştüre bilirim IEnumerable interface ini implement etmiş bir class a caste yapa bilirim
        Task<CustomResponseDto<NoContentDto>> UpdateAsync(Dto dto);// task diyerek async ye gönüştürüyoruz
        Task<CustomResponseDto<NoContentDto>> RemoveAsync(int id);
        Task<CustomResponseDto<NoContentDto>> RemoveRangeAsync(IEnumerable<int> ids);
    }
}
