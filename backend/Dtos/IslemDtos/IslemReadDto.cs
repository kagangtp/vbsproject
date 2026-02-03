namespace İlkProjem.backend.Dtos.IslemDtos;

public class IslemReadDto
{
    public int Id { get; set; }
    public required string Aciklama { get; set; }
    public decimal Miktar { get; set; }
    public string Tip { get; set; } = string.Empty;
    public string FormatliTarih { get; set; } = string.Empty;
}