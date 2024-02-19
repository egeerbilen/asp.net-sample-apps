using RedisSample.Interfaces;
using RedisSample.Services;
using StackExchange.Redis;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;
var multiplexer = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis:Host"));

builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
builder.Services.AddSingleton<IBaseCacheService, BaseCacheService>(); // AddSingleton ile baðýmlýlýklarý ekledik
builder.Services.AddSingleton<ICategoryService, CategoryService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
