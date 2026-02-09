using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using IlkProjem.DAL.Data;
using IlkProjem.BLL.Services;
using IlkProjem.DAL.Repositories;

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

// 2. HTTP İstek Hattını (Pipeline) Yapılandır

    // Geliştirme aşamasında Swagger arayüzünü açar
    app.UseSwagger();
    app.UseSwaggerUI();


app.UseRouting(); // Önce yönlendirme

// UseCors MUTLAKA UseRouting'den sonra, UseAuthorization'dan ÖNCE gelmeli
app.UseCors("AllowAngular");
// HTTPS yönlendirmesi (Güvenlik için)
app.UseHttpsRedirection();

// Controller'ları URL'ler ile eşleştir (Routing)
app.MapControllers();

// Uygulamayı başlat ve dinlemeye geç
// Uygulamanın hangi portu dinleyeceğini buraya elle yazıyoruz
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Veritabanı boşsa 50 tane sahte müşteri ekle
    if (!context.Customers.Any())
    {
        var fakeCustomers = CustomerSeeder.GetFakeCustomers(50);
        context.Customers.AddRange(fakeCustomers);
        context.SaveChanges();
    }
}


app.Run("http://localhost:5005");

