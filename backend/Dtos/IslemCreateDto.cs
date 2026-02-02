namespace Backend.Dtos;
using İlkProjem.backend.Models;

public class IslemCreateDto
{
    public required string Aciklama { get; set; }
    public decimal Miktar { get; set; }
    public IslemTipi Tip { get; set; } // Enum (0: Gelir, 1: Gider gibi)
}