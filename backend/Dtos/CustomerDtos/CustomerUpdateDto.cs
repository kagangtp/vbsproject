namespace İlkProjem.backend.Dtos.CustomerDtos;

public class CustomerUpdateDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Email { get; set; }
}