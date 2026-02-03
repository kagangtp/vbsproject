namespace İlkProjem.backend.Models;

//Enum Tanımı
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
    public IslemTipi Tip { get; set; } 
    public DateTime Tarih { get; set; }
}