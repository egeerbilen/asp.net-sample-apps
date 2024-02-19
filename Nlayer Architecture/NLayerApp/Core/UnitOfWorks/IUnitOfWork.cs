using System;

namespace Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        // Bu desen genellikle bir veritabanı işlemi boyunca bir dizi işlemi birleştirir ve bunları tek bir işlem gibi yönetir.Bu sayede işlemler arasında tutarlılık sağlanır ve veritabanı işlemleri daha yönetilebilir hale gelir.
        // IUnitOfWork, genellikle bir veritabanı işlemi için bir dizi CRUD (Create, Read, Update, Delete) işlemini içeren metotları içerir.
        Task CommitAsync();
        void Commit();
    }
}
