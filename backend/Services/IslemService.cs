namespace İlkProjem.backend.Services;

using İlkProjem.backend.Data;
using İlkProjem.backend.Models;
using System.Linq;

public class IslemService
{
    // Veriler program açık kaldığı sürece bu listede durur
    private static List<Islem> _islemler = VeriAmbori.VarsayilanIslemleriGetir();
    // Ekleme (Create)
    public void Ekle(Islem yeniIslem)
    {
        _islemler.Add(yeniIslem);
    }

    // Listeleme (Read)
    public List<Islem> HepsiniGetir() => _islemler;

    // Silme (Delete)
    public void Sil(int id)
    {
        var islem = _islemler.FirstOrDefault(i => i.Id == id);
        if (islem != null) _islemler.Remove(islem);
    }

    // Toplam Bakiye Hesaplama (Ekstra bir iş mantığı)
    public decimal BakiyeHesapla()
{
    // Artık tırnak içinde "Gelir" yerine IslemTipi.Gelir kullanıyoruz
    decimal gelir = _islemler
        .Where(i => i.Tip == IslemTipi.Gelir)
        .Sum(i => i.Miktar);

    decimal gider = _islemler
        .Where(i => i.Tip == IslemTipi.Gider)
        .Sum(i => i.Miktar);

    return gelir - gider;
}
}