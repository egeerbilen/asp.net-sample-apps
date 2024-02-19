using Autofac;
using Core.Repositories;
using Core.Services;
using Core.UnitOfWorks;
using NLayer.Caching;
using NLayer.Core.Services;
using Repository;
using Repository.Repositories;
using Repository.UnitOfWork;
using Service.Mapping;
using Service.Services;
using System.Reflection;
using Module = Autofac.Module;

namespace NLayer.API.Modules
{
    public class RepoServiceModule : Module
    {
        // Program.cs içerisini kirletmemek adına bunu yaptık
        protected override void Load(ContainerBuilder builder)
        {
            // unit of work ile genericrepository bir tane va o yüzden burada elle ekliyoruz
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(GenericService<,>)).As(typeof(IGenericService<,>)).InstancePerLifetimeScope();
            // ServiceWithDto<,> iki tane Generic aldığı için 1 tane vürgül koy 3 tane alsa 2 tane virgül konur

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();



            var apiAssembly = Assembly.GetExecutingAssembly();
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile));

            // sonu Repository ile bitenleri al ve sonu Repository ile biten interfaceleri de implement et aynısını birde servis için imp et
            // eğer Service veya Repository ile bitmez ise kendimiz ekleyeceğiz InstancePerLifetimeScope dediğimiz yerler kendimöiz eklendi üsttarafta örneği var
            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();

            // Cache eklemek istiyorsan aşağıdaki commenti aç
            // ben burada Service ile bitmediği için manuel olarak eklemek zorundayım
            // ProductWithCachingRepository olmadığı için aşağıda mecburen eklemek zorundayız

            // IProductCachingService sonu service ile bittiği için zaten yukarıda otomatik olarak algışıyor o yüzden gerek yok aşşağıdaki satıra
            // builder.RegisterType<ProductWithCachingService>().As<IProductCachingService>(); // IProductWithCachingService gördüğün zaman ProductWithCachingService i al diyoruz burada

        }
    }
}
