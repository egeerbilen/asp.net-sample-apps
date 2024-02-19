var builder = WebApplication.CreateBuilder(args);

// CORS (Cross-Origin Resource Sharing) politikas�n� yap�land�r
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder => builder.WithOrigins("http://localhost:4200") // Angular uygulaman�z�n adresini ekleyin
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

// Servisleri konteynere ekle.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SignalR servislerini ekle
builder.Services.AddSignalR();
builder.Services.AddControllers();

var app = builder.Build();

// HTTP iste�i i�leme hatt�n� yap�land�r.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseCors("AllowOrigin"); // Yukar�da tan�mlanan CORS politikas�n� kullan

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chatHub"); // Bu sat�r SignalR hub'�n� ekle
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
