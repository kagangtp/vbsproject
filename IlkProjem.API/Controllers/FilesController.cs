using Microsoft.AspNetCore.Mvc;
using IlkProjem.BLL.Services;

namespace IlkProjem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFilesService _filesService;

    public FilesController(IFilesService filesService)
    {
        _filesService = filesService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Dosya seçilmedi.");

        var result = await _filesService.UploadAsync(file);
        return Ok(result); // Yüklenen dosyanın bilgilerini (ID, Path vb.) döner
    }
}