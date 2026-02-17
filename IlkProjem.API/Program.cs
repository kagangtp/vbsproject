using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using IlkProjem.DAL.Data;
using IlkProjem.BLL.Services;
using IlkProjem.DAL.Repositories;
using IlkProjem.BLL.Interfaces;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. LOCALIZATION SETUP ---
builder.Services.AddLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultures = new[] { "en-US", "tr-TR" };
    options.DefaultRequestCulture = new RequestCulture("tr-TR"); // Architect's preference
    options.SupportedCultures = cultures.Select(c => new CultureInfo(c)).ToList();
    options.SupportedUICultures = cultures.Select(c => new CultureInfo(c)).ToList();

    // Prioritize Accept-Language header over QueryStrings
    options.RequestCultureProviders.Insert(0, new AcceptLanguageHeaderRequestCultureProvider());
});

// --- 2. OPENAPI / SWAGGER SETUP ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Standard Swagger
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, ct) =>
    {
        var localizer = context.ApplicationServices.GetRequiredService<IStringLocalizer<Program>>();
        document.Info.Title = localizer["ApiTitle"];
        return Task.CompletedTask;
    });
});

// --- 3. DATABASE & SERVICES (BLL/DAL) ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true
    };
});    

// Dependency Injection matching your Business/DataAccess layers
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICalculatorService, CalculatorService>();

builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// --- 4. CORS (Allowing Angular SPA) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()); // Important for Accept-Language header
});

var app = builder.Build();

// --- 5. MIDDLEWARE PIPELINE ("Interceptors") ---
// This acts as the server-side interceptor for Content Language
var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowAngular");

app.UseAuthentication();    // "Sen kimsin?" (JWT kontrolü)
app.UseAuthorization();     // "Bunu görmeye yetkin var mı?"

app.UseHttpsRedirection();
app.MapControllers();

// --- 6. SEED DATA ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!context.Customers.Any())
    {
        context.Customers.AddRange(CustomerSeeder.GetFakeCustomers(50));
        context.SaveChanges();
    }
}

app.MapOpenApi();
app.Run("http://localhost:5005");