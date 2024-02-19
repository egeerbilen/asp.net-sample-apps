
using FluentValidation.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

#pragma warning disable CS0618 // Type or member is obsolete
// Add services to the container.
builder.Services.AddControllers().AddFluentValidation(options =>
{
    options.ImplicitlyValidateChildProperties = true; // Nesnenin i�indeki di�er nesneleri de do�rular.
    options.ImplicitlyValidateRootCollectionElements = true; // bir liste veya dizi i�indeki ��eleri do�rular.

    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()); // bu uygulama i�indeki t�m do�rulama kurallar� otomatik olarak tan�mlanm�� olan s�n�flardan al�n�r.
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