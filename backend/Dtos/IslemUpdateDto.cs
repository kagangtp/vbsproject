namespace Backend.Dtos;

public class IslemUpdateDto
{
    // Güncelleme sırasında ID genellikle URL'den alınır ama 
    // güvenlik için DTO içinde de tutulabilir.
    public required string Aciklama { get; set; }
    public decimal Miktar { get; set; }
    
    // Not: Tip (Gelir/Gider) genelde bir kez belirlenir. 
    // Eğer değiştirilmesini istemiyorsan buraya eklemezsin.
}