using Microsoft.EntityFrameworkCore;
using IlkProjem.Core.Models;

namespace IlkProjem.DAL.Data;

// This class is the "Bridge" to PostgreSQL
public class AppDbContext : DbContext
{
    // The constructor tells EF how to connect using the settings in appsettings.json
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // This creates the "Customers" table in your database
    public DbSet<Customer> Customers => Set<Customer>();
}