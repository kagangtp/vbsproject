namespace Backend.Dtos;

public class IslemReadDto
{
    public int Id { get; set; }
    public required string Aciklama { get; set; }
    public decimal Miktar { get; set; }
    public string Tip { get; set; } = string.Empty; // "Gelir" veya "Gider" metni
    public string FormatliTarih { get; set; } = string.Empty; // "02 Şubat 2026" gibi
}