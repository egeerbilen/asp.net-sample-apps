// .Net6 da Startup dosyas� ortadan kalkt� starup dosyas�ndaki kodlar program.cs dosyas�na geldi
// ibr �ey global ise Program.cs dosyas� i�erisine yazmam�z gerekiyor

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
// ValidateFilterAttribute de�erini her bir controllessa tek tek eklemek yerine AddControllers(options => options.Filters.Add(new ValidateFilterAttribute()))
// �eklinde ekleye biliriz
// arrow functionlarda e�er tek sat�r varsa {} lere ihtiya� yoktur
builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>()); // validatoru buraya ekledik

//bizim bu i� i�in kendi filt�r �m�z var diyoruz
//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.SuppressModelStateInvalidFilter = true; //Frame work�n kendi d�nm�� oldu�u SuppressModelStateInvalidFilter bask�lad�k
//});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Cache eklemek istiyorsan a�a��daki commenti a�
builder.Services.AddMemoryCache();

// Filtreleme i�lemlarimizi burada yapt�k
builder.Services.AddScoped(typeof(NotFoundFilter<>)); // Generic oldu�u i�in typeof ile i�ine giriyorum NotFoundFilter diyoruz generic oldu�u i�ide <> kapad�k

// A�A�IDAK� T�M COMMENTLER RepoServiceeModule i�inde eklendi ve kod daha temiz hale getirildi
// Bu a�a��daki kodlar kodlar C# programlama dilinde kullan�lan ASP.NET Core uygulamalar�nda hizmet ba�lama (dependency injection) i�lemlerini yap�land�rmak i�in kullan�l�r.
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); // typeof, C# dilinde bir t�r�n tipini almak i�in kullan�lan bir operat�rd�r. 
//builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));


//builder.Services.AddScoped<IProductRepository, ProductRepository>(); // -> dependency injectionu buralara eklemek zorunday�z dependency ba��ml�l�klar�n� burada belirtiyoruz
//builder.Services.AddScoped<IProductService, ProductService>();

//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddAutoMapper(typeof(MapProfile));


builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), options =>
    {
        // SqlConnection'� Repository de kulland���m�z i�in bunu Repository katman�na bildiriyoruz
        // options.MigrationsAssembly("Repository"); -> bu tip g�venli de�il yani ilerde repository ismi de�i�tirmem gerekirse buray� da de�i�tirmem gerek
        options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name); // Tip g�venli hale geldi bu
    });
});

// Migration olu�turmak i�in kullanmam�z gereken komutlar
// 1- Package manager Consolu a� 
// 2- Default project: Repository se�ili olsun
// API se�ili olsun (Microsoft.EntityFrameworkCore.Desig ekli olmas� gerek bunu sadece 1 kere ekliyoruz dbcontext assebly den ayr� bir yerde oldu�u i�in gerekli)
// add-migration initial
// Migration dosyas� olu�turur sonras�nda
// snapshot dosyas� ile kar��la�t�r�r (bu �ndceden add-migration dedi�mizde snapshot al�r sonra eklenen table s�tunu varsa bulup ekler)
// update-database


// RepoServiceeModule � buraya ekliyoruz
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

app.UseHttpsRedirection(); // https y�nledirmesi

app.UseCustomException(); // bu bizim ekledi�imiz hata katman� bu hata katman�n�n �st tarafta olmas� �nemli

app.UseAuthorization(); // bir request geldi�inde token do�rulamas� bu midleware de ger�ekle�ir

app.MapControllers();

app.Run();
