namespace İlkProjem.backend.Models;

// 1. Enum Tanımı
public enum IslemTipi
{
    Gelir,
    Gider
}

public class Islem
{
    public int Id { get; set; }
    public required string Aciklama { get; set; }
    public decimal Miktar { get; set; }
    
    // 2. Property Artık String Değil, IslemTipi
    public IslemTipi Tip { get; set; } 
    public DateTime Tarih { get; set; }
}