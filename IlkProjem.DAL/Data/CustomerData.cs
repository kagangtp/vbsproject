using Microsoft.EntityFrameworkCore;
using IlkProjem.Core.Models;
using System.Linq.Expressions;

namespace IlkProjem.DAL.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Files> Files => Set<Files>(); // Yeni tablo

    public DbSet<House> Houses => Set<House>();
    public DbSet<Car> Cars => Set<Car>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Files Tablosu Yapılandırması (Kararlarımıza Uygun)
        modelBuilder.Entity<Files>(entity =>
        {
            entity.ToTable("Files"); // Tablo ismini netleştiriyoruz

            // PostgreSQL'e özel JSONB kolon tipi
            entity.Property(e => e.Metadata)
                  .HasColumnType("jsonb");

            // Performans için Index'ler
            entity.HasIndex(e => e.CreatedAt); 
            entity.HasIndex(e => e.RelativePath).IsUnique(); // Çakışmayı önlemek için
        });

        // 2. Customer Tablosu İsimlendirme Güncellemesi
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers"); // "CustomerData" vb. sorunlu isimleri düzeltir
        });

        // 3. RefreshToken Yapılandırması (Mevcut yapın)
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasOne(rt => rt.User)
                  .WithMany(u => u.RefreshTokens)
                  .HasForeignKey(rt => rt.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(rt => rt.Token).IsUnique();
        });

        // 4. Global Query Filter: BaseEntity'den türeyen tüm entity'ler için
        //    IsDeleted == false olanları otomatik filtreler
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                var falseConstant = Expression.Constant(false);
                var filter = Expression.Lambda(Expression.Equal(property, falseConstant), parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }
    }
}