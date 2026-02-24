using Microsoft.AspNetCore.Http;
using IlkProjem.Core.Models;

namespace IlkProjem.BLL.Services;

public interface IFilesService
{
    Task<Files> UploadAsync(IFormFile file);
    Task<byte[]> DownloadAsync(Guid fileId);
    Task<bool> DeleteAsync(Guid fileId);
}