using FileSignatures;
using IlkProjem.Core.Models;
using IlkProjem.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration; // Ayarlar için
using System.Text.Json;

namespace IlkProjem.BLL.Services;

public class FilesService : IFilesService
{
    private readonly IFilesRepository _fileRepository;
    private readonly string _uploadRoot;

    public FilesService(IFilesRepository fileRepository, IConfiguration configuration)
    {
        _fileRepository = fileRepository;
        
        // 1. ADIM: appsettings.json'daki yolu oku (Örn: /Users/kagan/Desktop/bankAppFiles)
        var pathFromConfig = configuration["FileSettings:StoragePath"];
        _uploadRoot = pathFromConfig ?? Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        if (!Directory.Exists(_uploadRoot))
            Directory.CreateDirectory(_uploadRoot);
    }

    public async Task<Files> UploadAsync(IFormFile file)
    {
        var inspector = new FileFormatInspector();
        using var stream = file.OpenReadStream();
        var format = inspector.DetermineFileFormat(stream);

        string detectedMime = format?.MediaType ?? "application/octet-stream";

        var now = DateTime.UtcNow;
        var relativeFolder = Path.Combine(now.Year.ToString(), now.Month.ToString("D2"));
        var physicalFolder = Path.Combine(_uploadRoot, relativeFolder);

        if (!Directory.Exists(physicalFolder))
            Directory.CreateDirectory(physicalFolder);

        var fileExtension = Path.GetExtension(file.FileName);
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var relativePath = Path.Combine(relativeFolder, uniqueFileName);
        var physicalPath = Path.Combine(physicalFolder, uniqueFileName);

        using (var fileStream = new FileStream(physicalPath, FileMode.Create))
        {
            stream.Position = 0; // Başa dönmeyi unutma!
            await stream.CopyToAsync(fileStream);
        }

        var fileEntity = new Files
        {
            Id = Guid.NewGuid(),
            FileName = file.FileName,
            MimeType = detectedMime,
            RelativePath = relativePath,
            FileSize = file.Length,
            Metadata = JsonSerializer.Serialize(new { OriginalName = file.FileName, Machine = Environment.MachineName }),
            CreatedAt = now
        };

        await _fileRepository.AddAsync(fileEntity);
        await _fileRepository.SaveChangesAsync();

        return fileEntity;
    }

    public async Task<byte[]> DownloadAsync(Guid fileId)
    {
        // 2. ADIM: Veri tabanından dosya bilgisini bul
        var fileRecord = await _fileRepository.GetByIdAsync(fileId);
        if (fileRecord == null) throw new FileNotFoundException("Dosya kaydı bulunamadı.");

        // Fiziksel yolu inşa et
        var physicalPath = Path.Combine(_uploadRoot, fileRecord.RelativePath);

        if (!File.Exists(physicalPath))
            throw new FileNotFoundException("Dosya diskte bulunamadı.");

        return await File.ReadAllBytesAsync(physicalPath);
    }

    public async Task<bool> DeleteAsync(Guid fileId)
    {
        // 3. ADIM: Hem DB hem Disk temizliği
        var fileRecord = await _fileRepository.GetByIdAsync(fileId);
        if (fileRecord == null) return false;

        var physicalPath = Path.Combine(_uploadRoot, fileRecord.RelativePath);

        // Önce fiziksel dosyayı sil
        if (File.Exists(physicalPath))
            File.Delete(physicalPath);

        // Sonra DB kaydını sil
        _fileRepository.Delete(fileRecord);
        await _fileRepository.SaveChangesAsync();

        return true;
    }
}