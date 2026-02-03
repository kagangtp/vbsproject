using İlkProjem.backend.Models;

namespace İlkProjem.backend.Data;

public static class VeriAmbori
{
    //Api initial verileri hardcoded.
    public static List<Islem> VarsayilanIslemleriGetir()
    {
        return new List<Islem>
        {
            new Islem { 
                Id = 1, 
                Aciklama = "Aylık Maaş", 
                Miktar = 45000.00m, 
                Tip = IslemTipi.Gelir, 
                Tarih = DateTime.Now.AddDays(-5) 
            },
            new Islem { 
                Id = 2, 
                Aciklama = "Kira Ödemesi", 
                Miktar = 15000.00m, 
                Tip = IslemTipi.Gider, 
                Tarih = DateTime.Now.AddDays(-3) 
            },
            new Islem { 
                Id = 3, 
                Aciklama = "Market Alışverişi", 
                Miktar = 2450.75m, 
                Tip = IslemTipi.Gider, 
                Tarih = DateTime.Now.AddDays(-1) 
            },
            new Islem { 
                Id = 4, 
                Aciklama = "Freelance Yazılım İşi", 
                Miktar = 8000.00m, 
                Tip = IslemTipi.Gelir, 
                Tarih = DateTime.Now 
            }
        };
    }
}