
using FluentValidation.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

#pragma warning disable CS0618 // Type or member is obsolete
// Add services to the container.
builder.Services.AddControllers().AddFluentValidation(options =>
{
    options.ImplicitlyValidateChildProperties = true; // Nesnenin içindeki diðer nesneleri de doðrular.
    options.ImplicitlyValidateRootCollectionElements = true; // bir liste veya dizi içindeki öðeleri doðrular.

    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()); // bu uygulama içindeki tüm doðrulama kurallarý otomatik olarak tanýmlanmýþ olan sýnýflardan alýnýr.
});
#pragma warning restore CS0618 // Type or member is obsolete

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();