using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using İlkProjem.backend.Data;
using İlkProjem.backend.Services;
using İlkProjem.backend.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Servisleri Konteynera Ekle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<CalculatorService>();

// Controller'ları ekle ve JSON ayarlarını yap
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Enum'ları string (metin) olarak okuyup yazabilmeyi sağlar
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }); // Controller yapısını kullanacağımızı belirtiyoruz


// Swagger/OpenAPI desteği (Test arayüzü için)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. HTTP İstek Hattını (Pipeline) Yapılandır

    // Geliştirme aşamasında Swagger arayüzünü açar
    app.UseSwagger();
    app.UseSwaggerUI();


// HTTPS yönlendirmesi (Güvenlik için)
app.UseHttpsRedirection();

// Controller'ları URL'ler ile eşleştir (Routing)
app.MapControllers();

// Uygulamayı başlat ve dinlemeye geç
// Uygulamanın hangi portu dinleyeceğini buraya elle yazıyoruz
app.Run("http://localhost:5005");

