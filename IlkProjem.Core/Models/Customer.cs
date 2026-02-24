namespace IlkProjem.Core.Models;

public class Customer
{
    public int Id { get; set; } 
    public required string Name { get; set; }
    public string? Email { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // --- DOSYA BAĞLANTISI BURADA BAŞLIYOR ---
    
    // 1. Foreign Key: Dosyanın Guid'ini burada tutacağız
    public Guid? ProfileImageId { get; set; } 

    // 2. Navigation Property: Kod tarafında direkt "customer.ProfileImage" diyebilmek için
    public virtual Files? ProfileImage { get; set; }

    public virtual ICollection<House> Houses { get; set; } = new List<House>();
    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}