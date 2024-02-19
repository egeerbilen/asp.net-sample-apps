// .Net6 da Startup dosyasý ortadan kalktý starup dosyasýndaki kodlar program.cs dosyasýna geldi
// ibr þey global ise Program.cs dosyasý içerisine yazmamýz gerekiyor

using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
using NLayer.API.Modules;
using NLayer.Service.Validations;
using Repository;
using Service.Mapping;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ValidateFilterAttribute deðerini her bir controllessa tek tek eklemek yerine AddControllers(options => options.Filters.Add(new ValidateFilterAttribute()))
// þeklinde ekleye biliriz
// arrow functionlarda eðer tek satýr varsa {} lere ihtiyaç yoktur
builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>()); // validatoru buraya ekledik

//bizim bu iþ için kendi filtýr ýmýz var diyoruz
//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.SuppressModelStateInvalidFilter = true; //Frame workün kendi dönmüþ olduðu SuppressModelStateInvalidFilter baskýladýk
//});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Cache eklemek istiyorsan aþaðýdaki commenti aç
builder.Services.AddMemoryCache();

// Filtreleme iþlemlarimizi burada yaptýk
builder.Services.AddScoped(typeof(NotFoundFilter<>)); // Generic olduðu için typeof ile içine giriyorum NotFoundFilter diyoruz generic olduðu içide <> kapadýk

// AÞAÐIDAKÝ TÜM COMMENTLER RepoServiceeModule içinde eklendi ve kod daha temiz hale getirildi
// Bu aþaðýdaki kodlar kodlar C# programlama dilinde kullanýlan ASP.NET Core uygulamalarýnda hizmet baðlama (dependency injection) iþlemlerini yapýlandýrmak için kullanýlýr.
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); // typeof, C# dilinde bir türün tipini almak için kullanýlan bir operatördür. 
//builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));


//builder.Services.AddScoped<IProductRepository, ProductRepository>(); // -> dependency injectionu buralara eklemek zorundayýz dependency baðýmlýlýklarýný burada belirtiyoruz
//builder.Services.AddScoped<IProductService, ProductService>();

//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddAutoMapper(typeof(MapProfile));


builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), options =>
    {
        // SqlConnection'ý Repository de kullandýðýmýz için bunu Repository katmanýna bildiriyoruz
        // options.MigrationsAssembly("Repository"); -> bu tip güvenli deðil yani ilerde repository ismi deðiþtirmem gerekirse burayý da deðiþtirmem gerek
        options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name); // Tip güvenli hale geldi bu
    });
});

// Migration oluþturmak için kullanmamýz gereken komutlar
// 1- Package manager Consolu aç 
// 2- Default project: Repository seçili olsun
// API seçili olsun (Microsoft.EntityFrameworkCore.Desig ekli olmasý gerek bunu sadece 1 kere ekliyoruz dbcontext assebly den ayrý bir yerde olduðu için gerekli)
// add-migration initial
// Migration dosyasý oluþturur sonrasýnda
// snapshot dosyasý ile karþýlaþtýrýr (bu öndceden add-migration dediðmizde snapshot alýr sonra eklenen table sütunu varsa bulup ekler)
// update-database


// RepoServiceeModule ü buraya ekliyoruz
builder.Host.UseServiceProviderFactory
    (new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // https yönledirmesi

app.UseCustomException(); // bu bizim eklediðimiz hata katmaný bu hata katmanýnýn üst tarafta olmasý önemli

app.UseAuthorization(); // bir request geldiðinde token doðrulamasý bu midleware de gerçekleþir

app.MapControllers();

app.Run();
