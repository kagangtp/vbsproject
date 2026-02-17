namespace IlkProjem.Core.Models;
public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string Email { get; set; }
    
    // Rolleri tutmak için: "Admin", "Manager", "Staff" vb.
    public string Role { get; set; } 
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}