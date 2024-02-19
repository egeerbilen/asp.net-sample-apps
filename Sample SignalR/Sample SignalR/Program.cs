var builder = WebApplication.CreateBuilder(args);

// CORS (Cross-Origin Resource Sharing) politikasýný yapýlandýr
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder => builder.WithOrigins("http://localhost:4200") // Angular uygulamanýzýn adresini ekleyin
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

// HTTP isteði iþleme hattýný yapýlandýr.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseCors("AllowOrigin"); // Yukarýda tanýmlanan CORS politikasýný kullan

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chatHub"); // Bu satýr SignalR hub'ýný ekle
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
