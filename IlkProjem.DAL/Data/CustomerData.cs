using Microsoft.EntityFrameworkCore;
using IlkProjem.Core.Models;

namespace IlkProjem.DAL.Data;

// This class is the "Bridge" to PostgreSQL
// Customer data name is a problem this needs an update
public class AppDbContext : DbContext
{
    // The constructor tells EF how to connect using the settings in appsettings.json
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // This creates the "Customers" table in your database
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            // User ile ilişki: 1 User → N RefreshToken
            entity.HasOne(rt => rt.User)
                  .WithMany(u => u.RefreshTokens)
                  .HasForeignKey(rt => rt.UserId)
                  .OnDelete(DeleteBehavior.Cascade); // User silinirse token'ları da git

            // Token sütununa index: hızlı arama için
            entity.HasIndex(rt => rt.Token).IsUnique();
        });
    }
}