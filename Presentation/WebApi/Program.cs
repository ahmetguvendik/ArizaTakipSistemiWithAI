using Persistance;

var builder = WebApplication.CreateBuilder(args);

// Gerekli servisleri ekle
builder.Services.AddControllers();            // ← Controller desteği
builder.Services.AddAuthorization();          // ← Authorization middleware çalışsın diye
builder.Services.AddEndpointsApiExplorer();   // ← Swagger için
builder.Services.AddSwaggerGen();             // ← Swagger için
builder.Services.AddPersistanceService();     // ← Kendi Persistance katmanını yüklüyorsun

var app = builder.Build();

// Swagger UI sadece development ortamında aktif olur
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Bu sıralama önemli değil ama genelde bu sırada olur
app.UseAuthorization();

app.MapControllers();  // ← Controller route'larını aktif hale getirir

app.Run();