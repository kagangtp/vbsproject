using İlkProjem.backend.Models;

namespace İlkProjem.backend.Dtos.IslemDtos;
public class IslemCreateDto
{
    public required string Aciklama { get; set; }
    public decimal Miktar { get; set; }
    public IslemTipi Tip { get; set; } // Enum (0: Gelir, 1: Gider gibi)
}