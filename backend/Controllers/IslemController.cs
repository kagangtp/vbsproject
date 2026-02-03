using Microsoft.AspNetCore.Mvc;
using İlkProjem.backend.Models; 
using İlkProjem.backend.Services;
using İlkProjem.backend.Dtos.IslemDtos;

namespace İlkProjem.backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IslemController : ControllerBase
{
    private readonly IslemService _islemService;

    public IslemController()
    {
        _islemService = new IslemService();
    }

    // 1. GET: api/islem -> Tüm listeyi READ DTO olarak döner
    [HttpGet]
    public IActionResult GetirHepsi()
    {
        var islemler = _islemService.HepsiniGetir();
        
        // Entity -> ReadDto Dönüşümü (Mapping)
        var sonuc = islemler.Select(i => new IslemReadDto 
        {
            Id = i.Id,
            Aciklama = i.Aciklama,
            Miktar = i.Miktar,
            Tip = i.Tip.ToString(),
            FormatliTarih = i.Tarih.ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("tr-TR"))
        }).ToList();

        return Ok(sonuc);
    }

    // 2. POST: api/islem -> CREATE DTO ile yeni işlem ekler
    [HttpPost]
    public IActionResult Ekle([FromBody] IslemCreateDto yeniIslemDto)
    {   
        var yeniIslem = new Islem 
        {
            Id = new Random().Next(100, 999),
            Aciklama = yeniIslemDto.Aciklama,
            Miktar = yeniIslemDto.Miktar,
            Tip = yeniIslemDto.Tip,
            Tarih = DateTime.Now // Tarihi backend otomatik atıyor
        };

        _islemService.Ekle(yeniIslem);
        
        return Ok(new { mesaj = "İşlem başarıyla eklendi.", id = yeniIslem.Id });
    }

    // 3. PUT: api/islem/{id} -> UPDATE DTO ile güncelleme yapar
    [HttpPut("{id}")]
    public IActionResult Guncelle(int id, [FromBody] IslemUpdateDto guncelIslemDto)
    {
        var mevcutIslem = _islemService.HepsiniGetir().FirstOrDefault(i => i.Id == id);
        
        if (mevcutIslem == null)
            return NotFound(new { hata = "Güncellenecek işlem bulunamadı." });

        // Sadece DTO'da izin verdiğimiz alanları güncelliyoruz
        mevcutIslem.Aciklama = guncelIslemDto.Aciklama;
        mevcutIslem.Miktar = guncelIslemDto.Miktar;

        return Ok(new { mesaj = "İşlem başarıyla güncellendi." });
    }

    // 4. DELETE: api/islem -> DELETE DTO (JSON Body) ile siler
    [HttpDelete]
    public IActionResult Sil([FromBody] IslemDeleteDto istek)
    {
        var varMi = _islemService.HepsiniGetir().Any(i => i.Id == istek.Id);
        
        if (!varMi)
            return NotFound(new { hata = $"{istek.Id} numaralı işlem bulunamadı." });

        _islemService.Sil(istek.Id);
        return Ok(new { mesaj = $"{istek.Id} ID'li işlem silindi." });
    }

    // 5. GET: api/islem/bakiye -> Bakiyeyi döner
    [HttpGet("bakiye")]
    public IActionResult BakiyeGor()
    {
        var bakiye = _islemService.BakiyeHesapla();
        return Ok(new { bakiye });
    }
}