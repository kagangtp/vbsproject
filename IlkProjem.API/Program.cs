using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using IlkProjem.DAL.Data;
using IlkProjem.BLL.Services;
using IlkProjem.DAL.Repositories;
using IlkProjem.BLL.Interfaces;
using IlkProjem.API.Filters;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Localization;
using IlkProjem.Core.Resources;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using System.Text.Json.Nodes;

var builder = WebApplication.CreateBuilder(args);

//
// ✅ Localization
//
builder.Services.AddLocalization();


builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultures = new[] { "en-US", "tr-TR" };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = Array.ConvertAll(cultures, c => new CultureInfo(c));
    options.SupportedUICultures = Array.ConvertAll(cultures, c => new CultureInfo(c));

    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
});
//
// ✅ OpenAPI
//
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, ct) =>
    {
        // 1. Localize the Title/Description as you did before
        var localizer = context.ApplicationServices.GetRequiredService<IStringLocalizer<Program>>();
        document.Info.Title = localizer["ApiTitle"];

        // 2. Add the Header to every operation (The v2.x "Filter" equivalent)
        foreach (var path in document.Paths.Values)
        {
            //
            // Might change later
            //Forging the "Accept-Language" header into every operation for Swagger UI
            foreach (var operation in path.Operations.Values)
            {
                operation.Parameters ??= [];
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Accept-Language",
                    In = ParameterLocation.Header,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = JsonSchemaType.String,
                        Enum = [
                            JsonValue.Create("en-US"),
                            JsonValue.Create("tr-TR")
                        ],
                        Default = JsonValue.Create("en-US")
                    }
                });
            }
        }
        return Task.CompletedTask;
    });
});



// 1. Servisleri Konteynera Ekle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICalculatorService,CalculatorService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// Controller'ları ekle ve JSON ayarlarını yap
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Enum'ları string (metin) olarak okuyup yazabilmeyi sağlar
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }); // Controller yapısını kullanacağımızı belirtiyoruz


// Swagger/OpenAPI desteği (Test arayüzü için)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<AcceptLanguageHeaderFilter>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

// 2. HTTP İstek Hattını (Pipeline) Yapılandır

    var locOptions = app.Services
    .GetRequiredService<IOptions<RequestLocalizationOptions>>();

app.UseRequestLocalization(locOptions.Value);

    // Geliştirme aşamasında Swagger arayüzünü açar
    app.UseSwagger();
    app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/openapi/v1.json?culture=en-US", "English");
    c.SwaggerEndpoint("/openapi/v1.json?culture=tr-TR", "Türkçe");
});


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

app.MapOpenApi();



app.Run("http://localhost:5005");

