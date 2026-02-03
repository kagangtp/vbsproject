namespace İlkProjem.backend.Models;

public class Customer
{
    public int Id { get; set; } // Birincil anahtar (Primary Key)
    public required string Name { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}